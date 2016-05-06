using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Web.Http;
using System.Web.Http.Controllers;
using System.Web.Http.Description;

//##### replace with project namespace #####
using SPA_Template.Areas.HelpPage;
using SPA_Template.Areas.HelpPage.ModelDescriptions;

namespace SPA_TemplateHelpers.Controllers.JavascriptGenerator
{
    //todo: generate startup.js partendo dalla cartella App (e relativo index.html)
    //todo: per i post method usare degli oggetti stub con newValues (self.newValue1 = ko.observable(self.value1))
    //todo: GenerateGrids, potrebbero esistere piu' metodi che ritornano le stesse collezioni di oggetti (in quel caso dovrei aggiungere piu' chiamate api una sotto l'altra)
    //todo: commonjs, nel caso di property liste di oggetti dovrebbe mapparli ad altri oggetti ko

    public class JavascriptGeneratorController : ApiController
    {
        #region Paths

        public static string AppPath
        {
            get
            {
                return Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "App");
            }
        }
        public static string AppGeneratePath
        {
            get
            {
                return Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "App-generate");
            }
        }
        public static string TemplatesPath
        {
            get
            {
                return Path.Combine(AppGeneratePath, "templates");
            }
        }
        public static string GeneratedPath
        {
            get
            {
                return Path.Combine(AppGeneratePath, "generated");
            }
        }

        
        //Template to start from
        public static string IndexTemplate
        {
            get
            {
                string filePath = Path.Combine(TemplatesPath, "Index.html");
                return File.ReadAllText(filePath);
            }
        }
        public static string StartupTemplate
        {
            get
            {
                string filePath = Path.Combine(TemplatesPath, "startup.js");
                return File.ReadAllText(filePath);
            }
        }
        public static string ApiTemplate
        {
            get
            {
                string filePath = Path.Combine(TemplatesPath, "api.js");
                return File.ReadAllText(filePath);
            }
        }
        public static string CommonTemplate
        {
            get
            {
                string filePath = Path.Combine(TemplatesPath, "common.js");
                return File.ReadAllText(filePath);
            }
        }
        public static string KoClientGridTemplateHtml
        {
            get
            {
                string filePath = Path.Combine(TemplatesPath, "ko-clientgrid-t1", "ko-clientgrid-t1.html");
                return File.ReadAllText(filePath);
            }
        }
        public static string KoClientGridTemplateJs
        {
            get
            {
                string filePath = Path.Combine(TemplatesPath, "ko-clientgrid-t1", "ko-clientgrid-t1.js");
                return File.ReadAllText(filePath);
            }
        }
        public static string KoPostTemplateHtml
        {
            get
            {
                string filePath = Path.Combine(TemplatesPath, "ko-post-t1", "ko-post-t1.html");
                return File.ReadAllText(filePath);
            }
        }
        public static string KoPostTemplateJs
        {
            get
            {
                string filePath = Path.Combine(TemplatesPath, "ko-post-t1", "ko-post-t1.js");
                return File.ReadAllText(filePath);
            }
        }


        //Where to place the generated files
        public static string ApiGeneratedPath
        {
            get
            {
                string filePath = Path.Combine(GeneratedPath, "api.js");
                return filePath;
            }
        }
        public static string CommonGeneratedPath
        {
            get
            {
                string filePath = Path.Combine(GeneratedPath, "common.js");
                return filePath;
            }
        }

        #endregion

        public IEnumerable<IGrouping<HttpControllerDescriptor, ApiDescription>> ApiControllers
        {
            get
            {
                //Group APIs by controller
                var model = GlobalConfiguration.Configuration.Services.GetApiExplorer().ApiDescriptions;
                ILookup<HttpControllerDescriptor, ApiDescription> apiGroups = model.ToLookup(api => api.ActionDescriptor.ControllerDescriptor);
                return apiGroups.OrderBy(p => p.Key.ControllerName)
                                .Where(p => p.Key.ControllerName != "JavascriptGenerator") //Exclude generation for this controller
                                .ToList();
            }
        }


#if !DEBUG
        [NonAction]
#endif
        [HttpGet]
        [Route("api/JavascriptGenerator/GenerateIndex")]
        public IHttpActionResult GenerateIndex()
        {
            var filesJs = Directory.EnumerateFiles(AppPath, "*.js", SearchOption.AllDirectories).OrderBy(p => p);

            //todo: tralasciare quelli con ComponentName null/empty e components-header

            var components = filesJs.Select(p => new ComponentStartupModel(p)).ToList();

            return Ok();
        }

        public class ComponentStartupModel
        {
            private string FilePath { get; set; }
            private string ComponentName { get; set; }
            public string RequirePath { get; set; } //   App/components/azienda-recap/azienda-recap
            public string ObservableName { get; set; } // self.attivaFatturaEdit = ko.observable();

            public string Params { get; set; } //todo: gestire params (sia in index.html che in route)

            //non valorizzata per components
            public string Route { get; set; } //   #/fatturazione/attiva/fatture/:id_fattura/edit
            /*
            this.get('#/fatturazione', function () {
                model.body(new BodyModel());
                model.body().ObservableName(true);  //({ code: this.params.code, userid: this.params.userid })
            });
            */

            public ComponentStartupModel(string FilePath)
            {
                this.FilePath = FilePath;
                this.ComponentName = GetComponentName(FilePath);
                this.RequirePath = GetRequiredPath();
                this.ObservableName = GetObservableName();
                this.Route = this.GetRoute();
            }

            private string GetComponentName(string FilePath)
            {
                if (FilePath == null)
                    return string.Empty;

                var directoryName = Path.GetDirectoryName(FilePath);
                if (directoryName == AppPath)
                    return string.Empty;


                var temp = GetComponentName(directoryName);
                if (temp == string.Empty)
                    return Path.GetFileName(directoryName);

                return GetComponentName(directoryName) + "-" + Path.GetFileName(directoryName);
            }

            private string GetRequiredPath()
            {
                var temp = Path.ChangeExtension(FilePath, string.Empty);
                temp = temp.Remove(temp.Length - 1); //rimuovo il punto finale


                temp = temp.Replace(AppDomain.CurrentDomain.BaseDirectory, string.Empty);

                return temp;
            }

            private string GetObservableName()
            {
                string temp = this.RequirePath;
                temp = Path.GetDirectoryName(temp);
                temp = temp.Replace("App", string.Empty);

                //todo: mancano le maiuscole
                temp = temp.Replace("-", string.Empty).Replace("\\", string.Empty);
                return temp;
            }

            private string GetRoute()
            {
                string temp = this.RequirePath;
                temp = Path.GetDirectoryName(temp);
                temp = temp.Replace("App", string.Empty);
                temp = "#" + temp.Replace("\\", "/");
                return temp;
            }
        }


#if !DEBUG
        [NonAction]
#endif
        [HttpGet]
        [Route("api/JavascriptGenerator/GenerateAPI")]
        public IHttpActionResult GenerateAPI()
        {
            string jsMethodsCall = string.Empty;
            foreach (var apiController in ApiControllers)
            {
                string controllerName = apiController.Key.ControllerName;

                //commento col nome del controller
                jsMethodsCall += Environment.NewLine + @"//" + controllerName + Environment.NewLine;

                foreach (var apiMethod in apiController)
                {
                    string relativePath = apiMethod.RelativePath;
                    string httpMethod = apiMethod.HttpMethod.Method;
                    string actionMethod = apiMethod.ActionDescriptor.ActionName;

                    string friendlyID = apiMethod.GetFriendlyId();
                    var apiModel = GlobalConfiguration.Configuration.GetHelpPageApiModel(friendlyID);

                    bool doesReturnJson = apiModel.ResourceDescription.ModelType.Name != "IHttpActionResult" ? true : false;
                    string doesReturnJsonString = doesReturnJson ? "true" : "false";


                    bool isGet = false;
                    if (httpMethod == "GET")
                        isGet = true;
                    else if (httpMethod != "POST")
                        continue;


                    string jsCurrentMethodCall = string.Empty;

                    // function GetAziende(divToBlock, success, error
                    jsCurrentMethodCall = @"function " + actionMethod + @"(divToBlock, success, error";

                    //parametri GET e POST
                    var parametriGet = new List<string>();
                    var parametriPost = new List<string>();
                    if (apiModel.UriParameters != null)
                        parametriGet.AddRange(apiModel.UriParameters.Select(p => p.Name));
                    if (apiModel.RequestBodyParameters != null)
                        parametriPost.AddRange(apiModel.RequestBodyParameters.Select(p => p.Name));

                    //aggiungo la virgola e lo spazio dopo error se ci sono parametri
                    //function GetAziende(divToBlock, success, error, 
                    if (parametriGet.Count > 0 || parametriPost.Count > 0)
                    {
                        jsCurrentMethodCall += ", ";

                        // function GetAziende(divToBlock, success, error, param1, param2) {
                        jsCurrentMethodCall += string.Join(", ", parametriGet);
                        jsCurrentMethodCall += string.Join(", ", parametriPost);
                    }
                    jsCurrentMethodCall += @") {" + Environment.NewLine;

                    //      Get(divToBlock, success, error, false, 
                    jsCurrentMethodCall += "\t"; //tab
                    jsCurrentMethodCall += isGet ? "Get" : "Post";
                    jsCurrentMethodCall += @"(divToBlock, success, error, " + doesReturnJsonString + ", ";


                    //url dell'api (coi parametri GET da inserire nell'url)
                    string apiUrl = "\"/"  // "/
                                    + relativePath.Replace("{", "\" + ").Replace("}", " + \"")  //api/metodo?param=asd&param2=asd
                                    + "\"";

                    //cancello l'ultimo pezzo  + "" diventa stringa vuota
                    apiUrl = apiUrl.Replace(" + \"\"", string.Empty);

                    //      Get(divToBlock, success, error, false, "api/metodo?param=asd&param2=asd"
                    jsCurrentMethodCall += apiUrl;


                    //      Get(divToBlock, success, error, false, "api/metodo?param=asd&param2=asd", { param1: param1, param2: param2 });
                    if (parametriPost.Count > 0)
                    {
                        jsCurrentMethodCall += ", { " + string.Join(", ", parametriPost.Select(p => p + ": " + p)) + " }";
                    }
                    jsCurrentMethodCall += @");" + Environment.NewLine;


                    //chiudo il metodo
                    jsCurrentMethodCall += @"};";


                    //lo aggiungo alle call
                    jsMethodsCall += jsCurrentMethodCall + Environment.NewLine;
                }
            }


            string nuovoTemplateApi = ApiTemplate;

            //replace dei metodi e delle chiamate ai metodi
            nuovoTemplateApi = nuovoTemplateApi.Replace("{METHODS_CALL}", jsMethodsCall);


            var methodsNameBuilder = new StringBuilder();
            foreach (var item in ApiControllers)
            {
                methodsNameBuilder.AppendLine(@"//" + item.Key.ControllerName);
                methodsNameBuilder.AppendLine(string.Join(", " + Environment.NewLine,
                                              item.Select(p => p.ActionDescriptor.ActionName + ": " + p.ActionDescriptor.ActionName))
                                              + ",");
                methodsNameBuilder.AppendLine();
            }
            string methodsName = methodsNameBuilder.ToString();
            methodsName = methodsName.Remove(methodsName.LastIndexOf(','), 1); //rimuovo l'ultima virgola

            nuovoTemplateApi = nuovoTemplateApi.Replace("{METHODS_NAME}", methodsName);


            File.WriteAllText(ApiGeneratedPath, nuovoTemplateApi);


            //check se ci sono nomi doppi
            var metodiDoppi = ApiControllers.SelectMany(p => p.Select(q => q.ActionDescriptor.ActionName))
                                       .GroupBy(p => p)
                                       .Select(p => new { Key = p.Key, Count = p.Count() })
                                       .Where(p => p.Count > 1)
                                       .ToList();
            if (metodiDoppi.Count > 0)
                return BadRequest("File API creato ma alcuni metodi sono doppi: " + string.Join(", ", metodiDoppi));


            var msgOk = "file api_generated.js generato, " + @"<br/>";
            string apiFilePath = System.IO.Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "App", "api.js");
            if (File.Exists(apiFilePath))
            {
                msgOk += "VS command window: Tools.DiffFiles " + ApiGeneratedPath + " " + apiFilePath;
            }
            return Ok(msgOk);
        }


#if !DEBUG
        [NonAction]
#endif
        [HttpGet]
        [Route("api/JavascriptGenerator/GenerateCommon")]
        public IHttpActionResult GenerateCommon()
        {
            var modelNames = new List<string>();
            string jsModels = string.Empty;
            foreach (var apiController in ApiControllers)
            {
                foreach (var apiMethod in apiController)
                {
                    //mi servono solo i metodi che ricevo in get
                    if (apiMethod.HttpMethod.Method != "GET")
                        continue;

                    string friendlyID = apiMethod.GetFriendlyId();
                    var apiModel = GlobalConfiguration.Configuration.GetHelpPageApiModel(friendlyID);


                    bool doesReturnJson = apiModel.ResourceDescription.ModelType.Name != "IHttpActionResult" ? true : false;

                    if (!doesReturnJson)
                        continue;

                    string nomeModello = apiModel.ResourceDescription.Name;
                    var modelCollection = apiModel.ResourceDescription as CollectionModelDescription;
                    if (modelCollection != null)
                    {
                        nomeModello = modelCollection.ElementDescription.Name;
                    }

                    //Usually I create model class with "Model" at the end
                    nomeModello = nomeModello.Replace("Model", string.Empty);

                    //Skip already added models (multiple method could return the same model)
                    if (modelNames.Contains(nomeModello))
                        continue;

                    modelNames.Add(nomeModello);

                    // function TipologiaPrestazione(TipologiaPrestazione) {
                    jsModels += "function " + nomeModello + "(" + nomeModello + ") {";

                    jsModels += Environment.NewLine;
                    jsModels += "\t";
                    jsModels += "var self = this;";
                    jsModels += Environment.NewLine + Environment.NewLine;

                    foreach (var prop in apiModel.ResourceProperties)
                    {
                        jsModels += "\t";

                        //se è un oggetto base:     self.id = TipologiaPrestazione.ID;
                        jsModels += "self." + ToJsName(prop.Name) + " = " + nomeModello + "." + prop.Name + ";";

                        jsModels += Environment.NewLine;
                    }

                    jsModels += "};";
                    jsModels += Environment.NewLine;
                    jsModels += Environment.NewLine;
                }
            }


            string nuovoTemplateCommon = CommonTemplate;

            //replace dei metodi e delle chiamate ai metodi
            nuovoTemplateCommon = nuovoTemplateCommon.Replace("{METHODS_CALL}", jsModels);

            nuovoTemplateCommon = nuovoTemplateCommon.Replace("{METHODS_NAME}",
                                                string.Join(", " + Environment.NewLine, modelNames.OrderBy(p => p).Select(p => p + ":" + p)));

            File.WriteAllText(CommonGeneratedPath, nuovoTemplateCommon);


            var msgOk = "file common_generated.js generato, " + @"<br/>";
            string commonFilePath = System.IO.Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "App", "common.js");
            if (File.Exists(commonFilePath))
            {
                msgOk += "VS command window: Tools.DiffFiles " + CommonGeneratedPath + " " + commonFilePath;
            }

            return Ok(msgOk);
        }


#if !DEBUG
        [NonAction]
#endif
        [HttpGet]
        [Route("api/JavascriptGenerator/GenerateGrids")]
        public IHttpActionResult GenerateGrids() //genero ko-grids per tutti metodi get che ritornano collection
        {
            string templateClientGridT1_js = KoClientGridTemplateJs;
            string templateClientGridT1_html = KoClientGridTemplateHtml;

            //cartella per metterci gli altri file generati
            Directory.CreateDirectory(GeneratedPath);

            foreach (var apiController in ApiControllers)
            {
                foreach (var apiMethod in apiController)
                {
                    if (apiMethod.HttpMethod.Method != "GET")
                        continue;

                    string friendlyID = apiMethod.GetFriendlyId();
                    var apiModel = GlobalConfiguration.Configuration.GetHelpPageApiModel(friendlyID);

                    bool doesReturnJson = apiModel.ResourceDescription.ModelType.Name != "IHttpActionResult" ? true : false;

                    if (!doesReturnJson)
                        continue;

                    string clientGridT1_js = templateClientGridT1_js;
                    string clientGridT1_html = templateClientGridT1_html;

                    string nomeModello = apiModel.ResourceDescription.Name;
                    var modelCollection = apiModel.ResourceDescription as CollectionModelDescription;
                    if (modelCollection == null)
                        continue;

                    var complexModel = modelCollection.ElementDescription as ComplexTypeModelDescription;
                    if (complexModel == null)
                        continue; //non genero knockout grids a partire da liste di oggetti base

                    nomeModello = modelCollection.ElementDescription.Name.Replace("Model", string.Empty).Trim();
                    if (string.IsNullOrWhiteSpace(nomeModello))
                        throw new Exception("Un Nome modello è vuoto o si chiama Model");

                    string nomeObsArray = ToJsName(nomeModello) //rendo minuscola la prima lettera
                                          + "s"; //pluralizzo aggiungendo una s alla fine

                    string nomeComponent = nomeObsArray + "-grid";

                    //replace dai template
                    clientGridT1_js = clientGridT1_js.Replace("nomeModello", nomeModello);
                    clientGridT1_js = clientGridT1_js.Replace("nomeObsArray", nomeObsArray);
                    clientGridT1_js = clientGridT1_js.Replace("ActionName", apiMethod.ActionDescriptor.ActionName);
                    clientGridT1_js = clientGridT1_js.Replace("ko-clientgrid-t1", nomeComponent); //per il define all'inizio per trovare il file html relativo


                    //genero le colonne per tutte le property (se sono complex una colonna per ogni sotto colonna, ad es. reparto.nome)
                    //todo: gestire checked per boolean e datetime
                    var paramsConNavi = GetGridColumnsSorting(complexModel);

                    var headerSortingParams = paramsConNavi.Select(p =>
                            "<th><a href='#' data-bind=\"click: function(){ nomeObsArrayPaginate().changeSort('" + p + "') }\">" + p + "</a></th>");

                    var headerDataParams = paramsConNavi.Select(p =>
                    "<td data-bind=\"text: " + p + "\"></td>");

                    string headerSortingHtml = string.Join(Environment.NewLine, headerSortingParams);
                    string headerDataHtml = string.Join(Environment.NewLine, headerDataParams);

                    clientGridT1_html = clientGridT1_html.Replace("headerSortingHtml", headerSortingHtml);
                    clientGridT1_html = clientGridT1_html.Replace("headerDataHtml", headerDataHtml);
                    clientGridT1_html = clientGridT1_html.Replace("nomeObsArray", nomeObsArray);

                    //salvo i file
                    string containerFolder = Path.Combine(GeneratedPath, "KoGrids", nomeComponent);
                    Directory.CreateDirectory(containerFolder);

                    string jsPath = Path.Combine(containerFolder, nomeComponent + ".js");
                    string htmlPath = Path.Combine(containerFolder, nomeComponent + ".html");
                    File.WriteAllText(jsPath, clientGridT1_js);
                    File.WriteAllText(htmlPath, clientGridT1_html);
                }
            }

            return Ok("Knockout Grids generated");
        }

        [NonAction]
        private List<string> GetGridColumnsSorting(ComplexTypeModelDescription ComplexModel)
        {
            var properties = new List<string>();
            foreach (var par in ComplexModel.Properties)
            {
                var simplePar = par.TypeDescription as SimpleTypeModelDescription;
                if (simplePar != null)
                {
                    properties.Add(ToJsName(par.Name));
                    continue;
                }

                //chiamata ricorsiva per complex model
                var innerComplexModel = par.TypeDescription as ComplexTypeModelDescription;
                if (innerComplexModel != null)
                {
                    var innerProperties = GetGridColumnsSorting(innerComplexModel);

                    //aggiungo navigazione (ad. es reparto.nome)
                    var innerPropertiesWithNavi = innerProperties.Select(p => ToJsName(par.Name) + "." + p);
                    properties.AddRange(innerPropertiesWithNavi);
                    continue;
                }


            }
            return properties;
        }


        [NonAction]
        private string ToJsName(string Testo)
        {
            //eccezioni
            if (Testo == "CAP")
                return Testo;

            if (Testo == "ID")
                return "id";

            if (Testo.StartsWith("ID"))
                return "id" + Testo.Substring(2);

            //altrimenti metto a minuscola solo la prima lettera
            return Testo.Substring(0, 1).ToLowerInvariant()
                                        + Testo.Substring(1, Testo.Length - 1);
        }
    }
}
