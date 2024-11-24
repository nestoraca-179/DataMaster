using DataMaster.Models;
using DevExpress.DataAccess.Sql;
using DevExpress.Web.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace DataMaster.Controllers
{
    public class RepController : Controller
    {
        RepStockArticulos report = new RepStockArticulos();
        public ActionResult RepStockArticulosPartial()
        {
            string connect = Session["CONNECT"].ToString();
            Empresa conn = Connection.GetConnByID(int.Parse(Session["ID_CONN"].ToString()));

            report.PB_Logo.ImageUrl = Request.Url.Scheme + "://" + Request.Url.Authority + "/" + conn.img;
            report.LBL_DescEmpresa.Text = conn.des_con;
            report.LBL_RIF.Text = conn.rif;
            report.LBL_Telf.Text = conn.tel;
            report.LBL_Direc.Text = conn.direc;

            SqlDataSource ds = report.DataSource as SqlDataSource;
            ds.Connection.ConnectionString = "XpoProvider=MSSqlServer;" + connect;

            return PartialView("~/Views/Report/_RepStockArticulosPartial.cshtml", report);
        }
        public ActionResult RepStockArticulosPartialExport()
        {
            return DocumentViewerExtension.ExportTo(report, Request);
        }
    }
}