//todo: cercare di togliere jQuery: https://www.airpair.com/knockout/posts/top-10-mistakes-knockoutjs
; (function (define) {
    define(['jquery', 'knockout'], function ($, ko) {
        return (function () {

            var knockoutgrids = {
                ClientGrid: ClientGrid,
                ServerGrid: ServerGrid
            };
            return knockoutgrids;

            function ClientGrid(Items, PageSize, SearchBy) {
                var self = this;

                self.items = ko.observableArray(Items); //array di oggetti da paginare

                self.pageSize = ko.observable(PageSize ? PageSize : 10); //numero di elementi per pagina
                self.paginaAttuale = ko.observable(0); //la pagina a cui mi trovo

                self.isEditingPageIndex = ko.observable(false);
                self.isEditingPageSize = ko.observable(false);
                self.goToEditPageIndex = function () {
                    self.isEditingPageSize(false);
                    self.isEditingPageIndex(true);
                };
                self.goToEditPageSize = function () {
                    self.isEditingPageIndex(false);
                    self.isEditingPageSize(true);
                };

                self.next = function () {
                    var newPageIndex = self.paginaAttuale() + 1;
                    if (newPageIndex >= self.pagesNumber()) {
                        newPageIndex = 0;
                    }
                    if (newPageIndex < 0) {
                        newPageIndex = self.pagesNumber() - 1;
                    }
                    self.paginaAttuale(newPageIndex);
                }
                self.previous = function () {
                    var newPageIndex = self.paginaAttuale() - 1;
                    if (newPageIndex >= self.pagesNumber()) {
                        newPageIndex = 0;
                    }
                    if (newPageIndex < 0) {
                        newPageIndex = self.pagesNumber() - 1;
                    }
                    self.paginaAttuale(newPageIndex);
                }

                self.pageSizePlus = function () {
                    var newpageSize = self.pageSize() + 1;
                    if (newpageSize < 0) {
                        newpageSize = 0;
                    }
                    if (newpageSize > self.items().length) {
                        newpageSize = self.items().length;
                    }
                    self.pageSize(newpageSize);
                }
                self.pageSizeMinus = function () {
                    var newpageSize = self.pageSize() - 1;
                    if (newpageSize < 0) {
                        newpageSize = 0;
                    }
                    if (newpageSize > self.items().length) {
                        newpageSize = self.items().length;
                    }
                    self.pageSize(newpageSize);
                }


                //filtro ricerca
                self.search = ko.observable(); //valore da filtrare
                self.searchBy = ko.observable(SearchBy); //nome della variabile da filtrare

                //filtering
                self.itemsInPageFiltered = ko.computed(function () {
                    if (!self.search() || !self.searchBy()) {
                        return self.items();
                    }
                    else {
                        return ko.utils.arrayFilter(self.items(), function (item) {

                            var searchName = self.searchBy();
                            var searchValue = null;
                            var testSplitted = searchName.split("."); //Appointments.name or appointments().name or appointments.name(), ...

                            ko.utils.arrayForEach(testSplitted, function (val) {
                                if (searchValue === null)
                                    searchValue = item[val];
                                else
                                    searchValue = searchValue[val];
                            });

                            //gestisco observable
                            while ($.isFunction(searchValue)) {
                                searchValue = searchValue();
                            }

                            var patternToTest = new RegExp(self.search(), "i");
                            return patternToTest.test(searchValue);
                        });
                    }
                });

                self.pagesNumber = ko.computed(function () {
                    return Math.ceil(self.itemsInPageFiltered().length / self.pageSize());
                }, self);

                //paging
                self.itemsInPage = ko.computed(function () {
                    self.isEditingPageSize(false);
                    self.isEditingPageIndex(false);
                    var startSkip = self.paginaAttuale() * self.pageSize();
                    var endSkip = startSkip + parseInt(self.pageSize());
                    return self.itemsInPageFiltered().slice(startSkip, endSkip);
                });

                //sorting
                self.directionDesc = ko.observable(true);
                self.sortby = ko.observable(''); //nome del parametro per cui sortare
                self.changeSort = function (sort) {
                    if (self.sortby() == sort) {
                        self.directionDesc(!self.directionDesc()); //cambio la direzione dell'ordinamento
                    }
                    self.sortby(sort);
                };
                self.itemsInPageSorted = ko.computed(function () {
                    if (!self.sortby()) {
                        return self.itemsInPage();
                    }
                    else {
                        self.items().sort(function (l, r) {
                            var leftValue = getValueFromSplits(l, self.sortby());
                            var rightValue = getValueFromSplits(r, self.sortby());

                            //handling null values order
                            if (leftValue == null) {
                                if (self.directionDesc() == true)
                                    return -1;
                                return 1;
                            }
                            if (rightValue == null) {
                                if (self.directionDesc() == true)
                                    return 1;
                                return -1;
                            }

                            //per gli oggetti custom
                            if (leftValue["orderByProp"]) {
                                console.log("orderByProp");
                                leftValue = leftValue["orderByProp"];
                                rightValue = rightValue["orderByProp"];
                            }

                            //per ordinare per observable
                            if ($.isFunction(leftValue)) {
                                leftValue = leftValue();
                                rightValue = rightValue();
                            }


                            if (leftValue === rightValue) {
                                return 0;
                            }
                            if (leftValue === '' && rightValue === '') {
                                return 0;
                            }

                            if (self.directionDesc() == true) {
                                return leftValue > rightValue ? 1 : -1;
                            }
                            else {
                                return leftValue < rightValue ? 1 : -1;
                            }
                        });
                        self.items.valueHasMutated(); //altrimenti non si refresha

                        return self.itemsInPage();
                    }
                });


                //seleziona tutti
                self.isAllSelected = ko.observable(false);
                self.isAllSelected.subscribe(function () {
                    //deseleziono tutti gli elementi
                    ko.utils.arrayForEach(self.items(), function (val) {
                        val.isSelected(false);
                    });

                    //se isAllSelected è true => riseleziono solo quelli nella pagina attuale
                    if (self.isAllSelected() === true) {
                        ko.utils.arrayForEach(self.itemsInPage(), function (val) {
                            val.isSelected(true);
                        });
                    }
                });

                //utils to split on dot and get value (es. observableObject().object.name => return name value)
                function getValueFromSplits(data, dataSplitPath) {
                    var splits = dataSplitPath.split(".");
                    if (splits.length == 1) //base step
                    {
                        if (data == null) {
                            return null;
                        }
                        return data[splits[0]];
                    }

                    var newData = data[splits[0]];
                    splits.splice(0, 1); //remove first element from array and rejoin the rest
                    return getValueFromSplits(newData, splits.join("."));
                }

                return;
            }

            //Mostra sempre tutti gli elementi ma non fa alcuna operazione, su cambio => chiamo OnChange
            function ServerGrid(Items, PageSize, SearchBy, OnChange) {
                var self = this;

                self.items = Items; //array di oggetti da paginare

                self.pageSize = ko.observable(PageSize ? PageSize : 10); //numero di elementi per pagina
                self.paginaAttuale = ko.observable(0); //la pagina a cui mi trovo


                self.isEditingPageIndex = ko.observable(false);
                self.isEditingPageSize = ko.observable(false);
                self.goToEditPageIndex = function () {
                    self.isEditingPageSize(false);
                    self.isEditingPageIndex(true);
                };
                self.goToEditPageSize = function () {
                    self.isEditingPageIndex(false);
                    self.isEditingPageSize(true);
                };

                self.next = function () {
                    var newPageIndex = self.paginaAttuale() + 1;
                    if (newPageIndex < 0) {
                        newPageIndex = 0;
                    }
                    self.paginaAttuale(newPageIndex);
                }
                self.previous = function () {
                    var newPageIndex = self.paginaAttuale() - 1;
                    if (newPageIndex < 0) {
                        newPageIndex = 0;
                    }
                    self.paginaAttuale(newPageIndex);
                }

                self.pageSizePlus = function () {
                    var newpageSize = self.pageSize() + 1;
                    if (newpageSize <= 0) {
                        newpageSize = 1;
                    }
                    self.pageSize(newpageSize);
                }
                self.pageSizeMinus = function () {
                    var newpageSize = self.pageSize() - 1;
                    if (newpageSize <= 0) {
                        newpageSize = 1;
                    }
                    self.pageSize(newpageSize);
                }


                //filtro ricerca
                self.search = ko.observable(); //valore da filtrare
                self.searchBy = ko.observable(SearchBy); //nome della variabile da filtrare

                //filtering
                self.itemsInPageFiltered = ko.computed(function () {
                    return self.items();
                });

                self.pagesNumber = ko.computed(function () {
                    return Math.ceil(self.itemsInPageFiltered().length / self.pageSize());
                }, self);

                //paging
                self.itemsInPage = ko.computed(function () {
                    return self.items();
                });

                //sorting
                self.directionDesc = ko.observable(true);
                self.sortby = ko.observable(''); //nome del parametro per cui sortare
                self.changeSort = function (sort) {
                    if (self.sortby() == sort) {
                        self.directionDesc(!self.directionDesc()); //cambio la direzione dell'ordinamento
                    }
                    self.sortby(sort);

                    self.refresh();
                };
                self.itemsInPageSorted = ko.computed(function () {
                    return self.items();
                });


                //seleziona tutti
                self.isAllSelected = ko.observable(false);
                self.isAllSelected.subscribe(function () {
                    //deseleziono tutti gli elementi
                    ko.utils.arrayForEach(self.items(), function (val) {
                        val.isSelected(false);
                    });

                    //se isAllSelected è true => riseleziono solo quelli nella pagina attuale
                    if (self.isAllSelected() === true) {
                        ko.utils.arrayForEach(self.itemsInPage(), function (val) {
                            val.isSelected(true);
                        });
                    }
                });


                //altri trigger per OnChange (oltre a changeSort)
                self.paginaAttuale.subscribe(function () {
                    self.refresh();
                });
                self.pageSize.subscribe(function () {
                    self.refresh();
                });
                self.search.subscribe(function () {
                    self.refresh();
                });

                self.refresh = function () {
                    OnChange(self.pageSize(), self.paginaAttuale(), self.sortby(), self.directionDesc(), self.searchBy(), self.search());
                };

                return;
            }  //Fine ServerGrid
        })();
    });
}(typeof define === 'function' && define.amd ? define : function (deps, factory) {
    if (typeof module !== 'undefined' && module.exports) { //Node
        module.exports = factory(require('jquery'), require('ko'));
    } else {
        window['knockoutgrids'] = factory(window['jQuery'], window['ko']);
    }
}));