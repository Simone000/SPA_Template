using SPA_Template.Areas.HelpPage;
using SPA_Template.Areas.HelpPage.ModelDescriptions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Web.Http.Controllers;
using System.Web.Http.Description;

namespace SPA_Template.Controllers
{
    //command window: Tools.DiffFiles D:\TFS\TFS_COSI\EasyMedMaster\EasyMedMaster\EasyMedMaster\App\Templates\api_generated.js D:\TFS\TFS_COSI\EasyMedMaster\EasyMedMaster\EasyMedMaster\App\api.js
    public class JavascriptGeneratorController : ApiController
    {
        //todo: metodi con formdata non hanno parametro

        //[NonAction] //da toglierla per poter generare il file
        [HttpGet]
        [Route("api/JavascriptGenerator/GenerateAPI")] //  /api/JavascriptGenerator/generateapi
        public IHttpActionResult GenerateAPI()
        {
            //Group APIs by controller
            var model = GlobalConfiguration.Configuration.Services.GetApiExplorer().ApiDescriptions;
            ILookup<HttpControllerDescriptor, ApiDescription> apiGroups = model.ToLookup(api => api.ActionDescriptor.ControllerDescriptor);

            string jsMethodsCall = string.Empty;
            foreach (var apiController in apiGroups)
            {
                string controllerName = apiController.Key.ControllerName;

                //commento col nome del controller
                jsMethodsCall += @"//" + controllerName + Environment.NewLine;

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

                    if (parametriGet.Count > 0 || parametriPost.Count > 0)
                    {
                        //aggiungo la virgola e lo spazio dopo error se ci sono parametri
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

                    //      Get(divToBlock, success, error, false, "api/metodo?param=asd&param2=asd"
                    jsCurrentMethodCall += apiUrl;


                    //      Get(divToBlock, success, error, false, "api/metodo?param=asd&param2=asd", { param1: param1, param2: param2 });
                    if (parametriPost.Count > 0)
                    {
                        jsCurrentMethodCall += ", {" + string.Join(", ", parametriPost.Select(p => p + ":" + p)) + "}";
                    }
                    jsCurrentMethodCall += @");" + Environment.NewLine;


                    //chiudo il metodo
                    jsCurrentMethodCall += @"};";


                    //lo aggiungo alle call
                    jsMethodsCall += jsCurrentMethodCall + Environment.NewLine;
                }
            }


            string templateBase = System.IO.File.ReadAllText(System.IO.Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "App", "Templates", "api_emptytemplate.js"));

            //replace dei metodi e delle chiamate ai metodi
            templateBase = templateBase.Replace("{METHODS_CALL}", jsMethodsCall);

            templateBase = templateBase.Replace("{METHODS_NAME}",
                                                string.Join(", " + Environment.NewLine,
                                                            apiGroups.SelectMany(p => p.Select(q => q.ActionDescriptor.ActionName + ":" + q.ActionDescriptor.ActionName))));


            System.IO.File.WriteAllText(System.IO.Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "App", "Templates", "api_generated.js"),
                                                    templateBase);


            //check se ci sono nomi doppi
            if (apiGroups.SelectMany(p => p.Select(q => q.ActionDescriptor.ActionName)).Count()
               !=
               apiGroups.SelectMany(p => p.Select(q => q.ActionDescriptor.ActionName)).Distinct().Count())
            {
                return BadRequest("File API creato ma alcuni metodi sono doppi!!!");
            }

            return Ok("file api_generated.js generato");
        }




        [HttpGet]
        [Route("api/JavascriptGenerator/GenerateCommon")]
        public IHttpActionResult GenerateCommon()
        {
            //Group APIs by controller
            var model = GlobalConfiguration.Configuration.Services.GetApiExplorer().ApiDescriptions;
            ILookup<HttpControllerDescriptor, ApiDescription> apiGroups = model.ToLookup(api => api.ActionDescriptor.ControllerDescriptor);

            var modelNames = new List<string>();
            string jsModels = string.Empty;
            foreach (var apiController in apiGroups)
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
                        jsModels += "self." + prop.Name.ToLowerInvariant() + " = " + nomeModello + "." + prop.Name + ";";




                        jsModels += Environment.NewLine;
                    }

                    jsModels += "};";
                    jsModels += Environment.NewLine;
                    jsModels += Environment.NewLine;
                }
            }


            string templateBase = System.IO.File.ReadAllText(System.IO.Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "App", "Templates", "common_emptytemplate.js"));

            //replace dei metodi e delle chiamate ai metodi
            templateBase = templateBase.Replace("{METHODS_CALL}", jsModels);

            templateBase = templateBase.Replace("{METHODS_NAME}",
                                                string.Join(", " + Environment.NewLine, modelNames.Select(p => p + ":" + p)));


            System.IO.File.WriteAllText(System.IO.Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "App", "Templates", "common_generated.js"),
                                                    templateBase);


            return Ok("file common_generated.js generato");
        }



        //[NonAction] //da toglierla per poter generare il file
        [HttpGet]
        [Route("api/JavascriptGenerator/GenerateAPI_OLD")] //  /api/JavascriptGenerator/generateapi
        public IHttpActionResult GenerateAPI_OLD()
        {
            //Group APIs by controller
            var model = GlobalConfiguration.Configuration.Services.GetApiExplorer().ApiDescriptions;
            ILookup<HttpControllerDescriptor, ApiDescription> apiGroups = model.ToLookup(api => api.ActionDescriptor.ControllerDescriptor);

            string javascriptMethodsAPI = string.Empty;
            foreach (var group in apiGroups)
            {
                string controllerName = group.Key.ControllerName;
                javascriptMethodsAPI += @"//" + controllerName + Environment.NewLine;

                foreach (var api in group)
                {
                    string relativePath = api.RelativePath;
                    string httpMethod = api.HttpMethod.Method;

                    string actionMethod = api.ActionDescriptor.ActionName;

                    string friendlyID = api.GetFriendlyId();
                    var apiModel = GlobalConfiguration.Configuration.GetHelpPageApiModel(friendlyID);

                    bool doesReturnJson = apiModel.ResourceDescription.ModelType.Name != "IHttpActionResult" ? true : false;
                    string doesReturnJsonString = doesReturnJson ? "true" : "false";

                    string currentJavascriptMethodsAPI = string.Empty;
                    if (httpMethod == "GET")
                    {
                        //function GetAziende(divToBlock, success, error ) {
                        currentJavascriptMethodsAPI += @"function " + actionMethod
                                             + @"(divToBlock, success, error"
                                             + (apiModel.UriParameters.Count > 0 ? ", " : "")
                                             + string.Join(", ", apiModel.UriParameters.Select(p => p.Name))
                                             + @") {"
                                             + Environment.NewLine;


                        //Get(divToBlock, success, error, true, "/api/Aziende/GetAziende");
                        currentJavascriptMethodsAPI += "\t"
                                             + @"Get(divToBlock, success, error, " + doesReturnJsonString + ", "

                                             + "\"/"
                                             + relativePath
                                               //.Replace("?", @"/" + actionMethod + @"/?")
                                               .Replace("{", "\" + ").Replace("}", " + \"")
                                             + "\");"

                                             + Environment.NewLine;


                        currentJavascriptMethodsAPI += @"};";
                    }

                    if (httpMethod == "POST")
                    {
                        //function UpdateAzienda(divToBlock, success, error, Nome, Descrizione) {
                        currentJavascriptMethodsAPI += @"function " + actionMethod
                                             + @"(divToBlock, success, error, "
                                             + string.Join(", ", apiModel.RequestBodyParameters.Select(p => p.Name))
                                             + @") {"
                                             + Environment.NewLine;

                        //Post(divToBlock, success, error, false, "/api/Aziende/UpdateAzienda", { Nome: Nome, Descrizione: Descrizione });
                        currentJavascriptMethodsAPI += "\t"
                                             + @"Post(divToBlock, success, error, " + doesReturnJsonString + " ,"

                                             + "\"/"
                                             + relativePath.Replace("{", "\" + ").Replace("}", " + \"")
                                             + @"/"
                                             + "\", "

                                             + "{"
                                             + string.Join(", ", apiModel.RequestBodyParameters.Select(p => p.Name + ":" + p.Name))
                                             + "}"

                                             + ");"
                                             + Environment.NewLine;


                        currentJavascriptMethodsAPI += @"};";
                    }


                    javascriptMethodsAPI += currentJavascriptMethodsAPI + Environment.NewLine;
                }
            }


            string templateBase = System.IO.File.ReadAllText(System.IO.Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "App", "Templates", "api_emptytemplate.js"));

            //replace dei metodi e delle chiamate ai metodi
            templateBase = templateBase.Replace("{METHODS_CALL}", javascriptMethodsAPI);

            templateBase = templateBase.Replace("{METHODS_NAME}",
                                                string.Join(", " + Environment.NewLine,
                                                            apiGroups.SelectMany(p => p.Select(q => q.ActionDescriptor.ActionName + ":" + q.ActionDescriptor.ActionName))));


            System.IO.File.WriteAllText(System.IO.Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "App", "Templates", "api_generated.js"),
                                                    templateBase);


            //check se ci sono nomi doppi
            if (apiGroups.SelectMany(p => p.Select(q => q.ActionDescriptor.ActionName)).Count()
               !=
               apiGroups.SelectMany(p => p.Select(q => q.ActionDescriptor.ActionName)).Distinct().Count())
            {
                throw new Exception("File API creato ma alcuni metodi sono doppi!!!");
            }


            //todo: considerare path completa (per web api in path diverse)

            return Ok("file api_generated.js generato");
        }
    }
}
