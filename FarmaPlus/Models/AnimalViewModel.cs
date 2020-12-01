using FarmaPlus.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace FarmaPlus
{
    public class AnimalViewModel
    {
        public int ZivotinjaID { get; set; }
        public int PosjednikID { get; set; }
        public int VrstaZivotinjeID { get; set; }
        public string OznakaUsneMarkice { get; set; }
        public System.DateTime? DatumUvoza { get; set; }
        public Nullable<int> GovedoID { get; set; }
        public Nullable<int> KonjID { get; set; }
        public Nullable<int> SvinjaID { get; set; }
        public Nullable<int> OvcaID { get; set; }
        public Nullable<int> KozaID { get; set; }
        public Nullable<int> MagaracID { get; set; }
        public Nullable<int> NojID { get; set; }
        public bool? Aktivna { get; set; }

        public string ZivotinjaIme { get; set; }
        public string VrstaZivotinjeNaziv { get; set; }
        public Spol ZivotinjaSpol { get; set; }


        public virtual tblGovedo tblGovedo { get; set; }
        public virtual tblPosjednik tblPosjednik { get; set; }
        public virtual tblVrstaZivotinje tblVrstaZivotinje { get; set; }
    }

    public enum Spol
    {
        M,
        Ž
    }
}