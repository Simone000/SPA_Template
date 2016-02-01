using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace SPA_Template.Controllers
{
    public class ODataController : Controller
    {
        /*
        self.aziendeTmp = ko.observableArray();
        self.tmpFiltroSearch = ko.observable('');
        self.tmpSortBy = ko.observable();
        self.tmpIsDesc = ko.observable(false);
        self.tmpFiltroServizio = ko.observable();
        self.tmpFiltroStatoAzienda = ko.observable();
        self.tmpCurrPage = ko.observable(0);
        self.tmpPageSize = ko.observable(20);

        self.loadTmp = function () {

            var url = 'http://localhost:56418/odata/AziendaServiziModels?$expand=Stato,Zona';
            
            if (self.tmpFiltroSearch() != '')
            {
                url += "&LikeParam=" + self.tmpFiltroSearch();
            }

            if (self.tmpSortBy())
            {
                url += "&$orderby=" + self.tmpSortBy();
                if (self.tmpIsDesc())
                    url += " desc";
            }

            $.ajax({
                type: "GET",
                contentType: "application/json; charset=utf-8",
                datatype: "json",
                url: url,
                beforeSend: function (XMLHttpRequest) {
                    //Specifying this header ensures that the results will be returned as JSON.
                    XMLHttpRequest.setRequestHeader("Accept", "application/json");
                },
                success: function (data) {
                    console.log("success");
                    self.aziendeTmp(ko.utils.arrayMap(data.value, function (item) {
                        return new common.AziendaServizi(item);
                    }));
                },
                error: function (jqXHR, textStatus, errorThrown) {
                    console.log("error");
                }
            });
        };
        self.loadTmp();
        
        */


        /*
        //http://localhost:56418/odata/AziendaServiziModels?$expand=Stato
        //http://localhost:56418/odata/AziendaServiziModels?$expand=Stato,Zona&$format=JSON
        // GET: odata/AziendaServiziModels
        [EnableQuery(AllowedQueryOptions = AllowedQueryOptions.All)]
        public async Task<IHttpActionResult> GetAziendaServiziModels(ODataQueryOptions<AziendaServiziModel> queryOptions, string LikeParam = null)
        {
            try
            {
                queryOptions.Validate(_validationSettings);
            }
            catch (ODataException ex)
            {
                return BadRequest(ex.Message);
            }


            var aziendeServizi_q = db.AziendeR.GetAziende(Enum_Servizi.Tutti)
                                              .OrderByDescending(p => p.DataUltimaModifica)
                                              .Include(p => p.esm_StatiAziende);

            //OData non supporta like
            if (!string.IsNullOrWhiteSpace(LikeParam))
            {
                aziendeServizi_q = aziendeServizi_q.Where(p => p.NomeOrganizzazione.Contains(LikeParam));
            }

            var aziendeServiziModels_q = TOMODELS.ToAziendeServiziModels().Invoke(aziendeServizi_q.AsExpandable());
            return Ok(aziendeServiziModels_q);
        }*/
    }
}