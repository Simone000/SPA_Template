﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Web.Http.Description;

namespace SPA_TemplateHelpers.Controllers
{
    public class TestJavascriptGeneratorController : ApiController
    {
        [HttpGet]
        [Route("GetAzienda")]
        public IHttpActionResult GetAzienda(int ID_Azienda, string Filtro)
        {
            return Ok();
        }

        [HttpGet]
        [Route("GetAziende")]
        [ResponseType(typeof(List<GetAziendaModel>))]
        public IHttpActionResult GetAziende()
        {
            var aziende = new List<GetAziendaModel>()
            {
                new GetAziendaModel()
                {
                    ID = 1,
                    Nome = "Azienda 1",
                    Citta = new CittaModel() { Nome = "San Paolo" },
                    Reparti = new List<RepartoModel>()
                    {
                        new RepartoModel()
                        {
                            ID = 1,
                            Desc = "reparto 1"
                        }
                    }
                },
                new GetAziendaModel()
                {
                    ID = 2,
                    Nome = "Azienda 2"
                }
            };

            return Ok(aziende);
        }

        [HttpPost]
        [Route("UpdateAzienda")]
        public IHttpActionResult UpdateAzienda(UpdateAziendaModel Model)
        {
            if (Model == null || !ModelState.IsValid)
                return BadRequest(ModelState);

            return Ok();
        }

        public class UpdateAziendaModel
        {
            [Required]
            public string Nome { get; set; }

            [MinLength(5, ErrorMessage = "Almeno lungo 5")]
            public string Descrizione { get; set; }
        }

        public class GetAziendaModel
        {
            public int ID { get; set; }
            public string Nome { get; set; }

            public List<RepartoModel> Reparti { get; set; }

            public CittaModel Citta { get; set; }
        }

        public class RepartoModel
        {
            public int ID { get; set; }
            public string Desc { get; set; }
        }

        public class CittaModel
        {
            public string Nome { get; set; }
        }
    }
}
