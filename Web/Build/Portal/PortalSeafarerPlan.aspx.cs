using System;
using System.Data;
using System.Web.UI.WebControls;
using SouthNests.Phoenix.Framework;
using SouthNests.Phoenix.Portal;
using System.Collections.Specialized;
using System.Web.UI.HtmlControls;
using SouthNests.Phoenix.Common;
using SouthNests.Phoenix.CrewManagement;
using Telerik.Web.UI;
using SouthNests.Phoenix.CrewOffshore;

public partial class Portal_PortalSeafarerPlan : PhoenixBasePage
{
    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {
            if (!IsPostBack)
            {
                ViewState["empid"] = "";
                ViewState["PAGENUMBER"] = 1;
                ViewState["SORTEXPRESSION"] = null;
                ViewState["SORTDIRECTION"] = null;
                ViewState["CURRENTINDEX"] = -1;
                if (Request.QueryString["empid"] != null && Request.QueryString["empid"].ToString() != "")
                    ViewState["empid"] = Request.QueryString["empid"].ToString();
                ifMoreInfo.Attributes["src"] = "../Portal/PortalSeafarerPlanProposed.aspx?empid="+ ViewState["empid"];
            }
        }
        catch(Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }
    protected void RadAjaxManager1_AjaxRequest(object sender, AjaxRequestEventArgs e)
    {
        if(e.Argument.ToUpper() == "HOME")
        {
            ifMoreInfo.Attributes["src"] = "../Dashboard/DashboardHome.aspx";
        }
        if (e.Argument.ToUpper() == "PLAN")
        {
            ifMoreInfo.Attributes["src"] = "../Portal/PortalSeafarerPlanProposed.aspx?empid=" + ViewState["empid"];
        }
        if (e.Argument.ToUpper() == "LETTEROFINTENT")
        {
            ifMoreInfo.Attributes["src"] = "../Portal/PortalSeafarerPlanLetterofIntent.aspx?empid=" + ViewState["empid"];
        }
        if (e.Argument.ToUpper() == "OFFERLETTER")
        {
            int iRowCount = 0;
            int iTotalPageCount = 0;
            string CrewPlanID = null;
            string sortexpression = (ViewState["SORTEXPRESSION"] == null) ? null : (ViewState["SORTEXPRESSION"].ToString());
            int? sortdirection = null;
            if (ViewState["SORTDIRECTION"] != null)
                sortdirection = Int32.Parse(ViewState["SORTDIRECTION"].ToString());

            DataSet ds = PhoenixPortalSeafarer.PortalSearchCrewPlan(Convert.ToInt32(ViewState["empid"].ToString()), sortexpression, sortdirection,
                                                                                      (int)ViewState["PAGENUMBER"],
                                                                                      General.ShowRecords(null),
                                                                                      ref iRowCount,
                                                                                      ref iTotalPageCount);
            if (ds.Tables[0].Rows.Count > 0)
            {
                CrewPlanID = ds.Tables[0].Rows[0]["FLDCREWPLANID"].ToString();
            }

            ViewState["ROWCOUNT"] = iRowCount;
            ViewState["TOTALPAGECOUNT"] = iTotalPageCount;
            ifMoreInfo.Attributes["src"] = "../Options/OptionCrewOffshoreOfferLetter.aspx?Crewplanid=" + CrewPlanID;
        } 
    }
    
}