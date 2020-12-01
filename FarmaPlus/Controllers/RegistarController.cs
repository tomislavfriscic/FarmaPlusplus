using FarmaPlus.Models;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Data.Entity.Validation;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace FarmaPlus.Controllers
{
    public class RegistarController : Controller
    {
        FarmaPlusEntities db = new FarmaPlusEntities();

        // GET: Registar
        public ActionResult Index()
        {
            var spolovi = new SelectList(new[] { "M", "Ž" });

            List<tblVrstaZivotinje> AnimalKind = db.tblVrstaZivotinje.ToList();

            ViewBag.Kinds = new SelectList(AnimalKind, "VrstaZivotinjeID", "Naziv");
            ViewBag.Spolovi = spolovi;

            return View();
        }

        public JsonResult GetAnimalList()
        {
            List<AnimalViewModel> AnimalList = null;
            AnimalList = db.tblZivotinja.Where(x => x.Aktivna == true).Select(x => new AnimalViewModel
            {
                ZivotinjaID = x.ZivotinjaID,
                PosjednikID = x.PosjednikID,
                VrstaZivotinjeID = x.tblVrstaZivotinje.VrstaZivotinjeID,
                OznakaUsneMarkice = x.OznakaUsneMarkice,
                DatumUvoza = x.DatumUvoza,
                GovedoID = x.tblGovedo.GovedoID,
                ZivotinjaIme = x.tblGovedo.Ime,
                VrstaZivotinjeNaziv = x.tblVrstaZivotinje.Naziv

                //ovo ce ici kad se dodaju preostale tablice za svaku vrstu zivotinje posebno
                //KonjID
                //SvinjaID
                //OvcaID 
                //KozaID 
                //MagaracID
                //NojID 


            }).ToList();



            return Json(AnimalList, JsonRequestBehavior.AllowGet);
        }

        public JsonResult GetAnimalById(int AnimalId)
        {
            tblZivotinja model = db.tblZivotinja.Where(x => x.ZivotinjaID == AnimalId).SingleOrDefault();

            model.tblGovedo = db.tblGovedo.Where(x => x.GovedoID == model.GovedoID).SingleOrDefault();

            string value = string.Empty;
            value = JsonConvert.SerializeObject(model, Formatting.Indented, new JsonSerializerSettings
            {
                ReferenceLoopHandling = ReferenceLoopHandling.Ignore
            });

            return Json(value, JsonRequestBehavior.AllowGet);
        }

        public JsonResult SaveDataInDatabase(AnimalViewModel model)
        {
            var result = false;

            int userID = Convert.ToInt32(Session["UserID"]);

            try
            {
                if (model.ZivotinjaID > 0)
                {
                    tblZivotinja zivotinja = db.tblZivotinja.SingleOrDefault(x => x.Aktivna == true && x.ZivotinjaID == model.ZivotinjaID);
                    zivotinja.tblGovedo = db.tblGovedo.SingleOrDefault(x => x.Aktivno == true && x.GovedoID == zivotinja.GovedoID);

                    zivotinja.PosjednikID = model.PosjednikID;
                    zivotinja.VrstaZivotinjeID = model.VrstaZivotinjeID;
                    zivotinja.OznakaUsneMarkice = model.OznakaUsneMarkice;
                    zivotinja.DatumUvoza = model.DatumUvoza;
                    zivotinja.GovedoID = model.GovedoID;
                    zivotinja.Aktivna = model.Aktivna;
                    //zivot.tblGovedo.KodDrzave = model.tblGovedo.KodDrzave;
                    //zivot.tblGovedo.ZivotniBrojGoveda = model.tblGovedo.ZivotniBrojGoveda;
                    zivotinja.tblGovedo.Ime = model.tblGovedo.Ime;
                    zivotinja.tblGovedo.DatumRodenja = model.tblGovedo.DatumRodenja;
                    if (!string.IsNullOrEmpty(model.tblGovedo.Spol))
                        zivotinja.tblGovedo.Spol = model.tblGovedo.Spol;
                    zivotinja.tblGovedo.Pasmina = model.tblGovedo.Pasmina;
                    zivotinja.tblGovedo.MajkaKodDrzave = model.tblGovedo.MajkaKodDrzave;
                    zivotinja.tblGovedo.MajkaZivotniBroj = model.tblGovedo.MajkaZivotniBroj;
                    zivotinja.tblGovedo.MajkaIme = model.tblGovedo.MajkaIme;
                    zivotinja.tblGovedo.OtacKodDrzave = model.tblGovedo.OtacKodDrzave;
                    zivotinja.tblGovedo.OtacHbBroj = model.tblGovedo.OtacHbBroj;
                    zivotinja.tblGovedo.OtacIme = model.tblGovedo.OtacIme;
                    zivotinja.tblGovedo.DatumUpisaURegistar = model.tblGovedo.DatumUpisaURegistar;
                    zivotinja.tblGovedo.Napomena = model.tblGovedo.Napomena;
                    zivotinja.tblGovedo.Aktivno = model.tblGovedo.Aktivno;

                    db.SaveChanges();
                    result = true;
                }
                else
                {
                    tblZivotinja zivotinja = new tblZivotinja();
                    zivotinja.tblGovedo = new tblGovedo();

                    zivotinja.PosjednikID = userID;
                    zivotinja.VrstaZivotinjeID = 1; //zasad dok se ne ubace i ostale životinje, 1 je za govedo
                    zivotinja.OznakaUsneMarkice = model.OznakaUsneMarkice;
                    zivotinja.DatumUvoza = model.DatumUvoza;
                    zivotinja.GovedoID = model.GovedoID;
                    zivotinja.Aktivna = true;
                    //zivot.tblGovedo.KodDrzave = model.tblGovedo.KodDrzave;
                    //zivot.tblGovedo.ZivotniBrojGoveda = model.tblGovedo.ZivotniBrojGoveda;
                    zivotinja.tblGovedo.Ime = model.tblGovedo.Ime;
                    zivotinja.tblGovedo.DatumRodenja = model.tblGovedo.DatumRodenja;
                    zivotinja.tblGovedo.Spol = model.tblGovedo.Spol;
                    zivotinja.tblGovedo.Pasmina = model.tblGovedo.Pasmina;
                    zivotinja.tblGovedo.MajkaKodDrzave = model.tblGovedo.MajkaKodDrzave;
                    zivotinja.tblGovedo.MajkaZivotniBroj = model.tblGovedo.MajkaZivotniBroj;
                    zivotinja.tblGovedo.MajkaIme = model.tblGovedo.MajkaIme;
                    zivotinja.tblGovedo.OtacKodDrzave = model.tblGovedo.OtacKodDrzave;
                    zivotinja.tblGovedo.OtacHbBroj = model.tblGovedo.OtacHbBroj;
                    zivotinja.tblGovedo.OtacIme = model.tblGovedo.OtacIme;
                    zivotinja.tblGovedo.DatumUpisaURegistar = model.tblGovedo.DatumUpisaURegistar;
                    zivotinja.tblGovedo.Napomena = model.tblGovedo.Napomena;
                    zivotinja.tblGovedo.Aktivno = true;

                    db.tblZivotinja.Add(zivotinja);
                    db.SaveChanges();
                    result = true;
                }
            }
            catch (DbEntityValidationException e)
            {
                foreach (var eve in e.EntityValidationErrors)
                {
                    Console.WriteLine("Entity of type \"{0}\" in state \"{1}\" has the following validation errors:",
                        eve.Entry.Entity.GetType().Name, eve.Entry.State);
                    foreach (var ve in eve.ValidationErrors)
                    {
                        Console.WriteLine("- Property: \"{0}\", Value: \"{1}\", Error: \"{2}\"",
                            ve.PropertyName,
                            eve.Entry.CurrentValues.GetValue<object>(ve.PropertyName),
                            ve.ErrorMessage);
                    }
                }
                throw;
            }

            return Json(result, JsonRequestBehavior.AllowGet);
        }

        public JsonResult DeleteAnimal(int ZivotinjaID)
        {
            bool result = false;

            tblZivotinja Ziv = db.tblZivotinja.SingleOrDefault(x => x.Aktivna == true && x.ZivotinjaID == ZivotinjaID);
            if (Ziv != null)
            {
                Ziv.Aktivna = false;
                Ziv.tblGovedo.Aktivno = false;
                db.SaveChanges();
                result = true;
            }

            return Json(result, JsonRequestBehavior.AllowGet);
        }


    }
}