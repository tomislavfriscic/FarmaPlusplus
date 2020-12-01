using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using FarmaPlus.Models;

namespace FarmaPlus.Controllers
{
    public class Registar2Controller : Controller
    {
        private FarmaPlusEntities db = new FarmaPlusEntities();
        private SelectList spolovi = new SelectList(new[] { "M", "Ž" });

        // GET: Registar2
        public ActionResult Index()
        {
            var tblZivotinja = db.tblZivotinja.Include(t => t.tblGovedo).Include(t => t.tblPosjednik).Include(t => t.tblVrstaZivotinje);
            return View(tblZivotinja.ToList());
        }

        // GET: Registar2/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            tblZivotinja zivotinja = db.tblZivotinja.Find(id);
            if (zivotinja == null)
            {
                return HttpNotFound();
            }

            return View(zivotinja);
        }

        // GET: Registar2/Create
        public ActionResult Create()
        {
            ViewBag.GovedoID = new SelectList(db.tblGovedo, "GovedoID", "Ime");
            ViewBag.PosjednikID = new SelectList(db.tblPosjednik, "PosjednikID", "Email");
            ViewBag.VrstaZivotinjeID = new SelectList(db.tblVrstaZivotinje, "VrstaZivotinjeID", "Naziv");

            ViewBag.Spolovi = new SelectList(spolovi);

            return View();
        }

        // POST: Registar2/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "ZivotinjaID,PosjednikID,VrstaZivotinjeID,OznakaUsneMarkice,DatumUvoza,GovedoID,KonjID,SvinjaID,OvcaID,KozaID,MagaracID,NojID,Aktivna, tblGovedo")] tblZivotinja model)
        {
            int userID = Convert.ToInt32(Session["UserID"]);

            if (userID > 0)
            {
                tblZivotinja zivotinja = new tblZivotinja
                {
                    tblGovedo = new tblGovedo(),

                    PosjednikID = userID,
                    VrstaZivotinjeID = 1, //zasad dok se ne ubace i ostale životinje, 1 je za govedo
                    OznakaUsneMarkice = model.OznakaUsneMarkice,
                    DatumUvoza = model.DatumUvoza,
                    GovedoID = model.GovedoID,
                    Aktivna = true
                };
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
                return RedirectToAction("Index");
            }

            ViewBag.GovedoID = new SelectList(db.tblGovedo, "GovedoID", "Ime", model.GovedoID);
            ViewBag.PosjednikID = new SelectList(db.tblPosjednik, "PosjednikID", "Email", model.PosjednikID);
            ViewBag.VrstaZivotinjeID = new SelectList(db.tblVrstaZivotinje, "VrstaZivotinjeID", "Naziv", model.VrstaZivotinjeID);
            return View(model);
        }

        // GET: Registar2/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            tblZivotinja zivotinja = db.tblZivotinja.Find(id);
            if (zivotinja == null)
            {
                return HttpNotFound();
            }
            ViewBag.GovedoID = new SelectList(db.tblGovedo, "GovedoID", "Ime", zivotinja.GovedoID);
            ViewBag.PosjednikID = new SelectList(db.tblPosjednik, "PosjednikID", "Email", zivotinja.PosjednikID);
            ViewBag.VrstaZivotinjeID = new SelectList(db.tblVrstaZivotinje, "VrstaZivotinjeID", "Naziv", zivotinja.VrstaZivotinjeID);

            var selected = spolovi.First(x => x.Text == zivotinja.tblGovedo.Spol);
            selected.Selected = true;
            var sl = new SelectList(spolovi, selected.Text);
            ViewBag.Spolovi = sl;

            return View(zivotinja);
        }

        // POST: Registar2/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "ZivotinjaID,PosjednikID,VrstaZivotinjeID,OznakaUsneMarkice,DatumUvoza,GovedoID,KonjID,SvinjaID,OvcaID,KozaID,MagaracID,NojID,Aktivna, tblGovedo")] tblZivotinja model)
        {
            if (Convert.ToInt32(Session["UserID"]) > 0)
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

                //db.Entry(model).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.GovedoID = new SelectList(db.tblGovedo, "GovedoID", "Ime", model.GovedoID);
            ViewBag.PosjednikID = new SelectList(db.tblPosjednik, "PosjednikID", "Email", model.PosjednikID);
            ViewBag.VrstaZivotinjeID = new SelectList(db.tblVrstaZivotinje, "VrstaZivotinjeID", "Naziv", model.VrstaZivotinjeID);
            return View(model);
        }

        // GET: Registar2/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            tblZivotinja tblZivotinja = db.tblZivotinja.Find(id);
            if (tblZivotinja == null)
            {
                return HttpNotFound();
            }
            return View(tblZivotinja);
        }

        // POST: Registar2/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            tblZivotinja tblZivotinja = db.tblZivotinja.Find(id);
            db.tblZivotinja.Remove(tblZivotinja);
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}
