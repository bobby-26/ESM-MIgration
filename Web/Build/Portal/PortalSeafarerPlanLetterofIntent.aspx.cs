using System;
using System.Data;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Telerik.Web.UI;
using SouthNests.Phoenix.Common;
using SouthNests.Phoenix.CrewOffshore;
using SouthNests.Phoenix.Framework;
using System.Web.UI.HtmlControls;
using SouthNests.Phoenix.Portal;

public partial class PortalSeafarerPlanLetterofIntent : PhoenixBasePage
{
    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {
            SessionUtil.PageAccessRights(this.ViewState);
            cmdHiddenSubmit.Attributes.Add("style", "display:none;");
            if (!IsPostBack)
            {
                ViewState["PAGENUMBER"] = 1;
                ViewState["SORTEXPRESSION"] = null;
                ViewState["SORTDIRECTION"] = null;
                ViewState["CURRENTINDEX"] = 1;
                //ViewState["ISRATING"] = "";
                ViewState["EMPID"] = "";
                //ViewState["NEWAPP"] = "";
                //ViewState["RANKPOSTED"] = "";
                //ViewState["letterid"] = "";
                if (Request.QueryString["empid"] != null && Request.QueryString["empid"].ToString() != "")
                    ViewState["EMPID"] = Request.QueryString["empid"].ToString();
                //SetEmployeePrimaryDetails();
                gvLetterOfInt.PageSize = int.Parse(PhoenixGeneralSettings.CurrentGeneralSetting.Records);
            }

            PhoenixToolbar tool = new PhoenixToolbar();
           // tool.AddButton("Home", "HOME", ToolBarDirection.Right);
            tool.AddButton("Plan", "PLAN", ToolBarDirection.Left);
            tool.AddButton("Letter Of Intent", "LETTEROFINTENT", ToolBarDirection.Left);
            tool.AddButton("Offer Letter", "OFFERLETTER", ToolBarDirection.Left);

            HomeMenu.MenuList = tool.Show();
            HomeMenu.SelectedMenuIndex = 1;
        }
        catch(Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }
    protected void cmdHiddenSubmit_Click(object sender, EventArgs e)
    {
        ViewState["PAGENUMBER"] = 1;
        BindData();
        gvLetterOfInt.Rebind();
    }
    private void BindData()
    {
        try
        {
            int iRowCount = 0;
            int iTotalPageCount = 0;

            string sortexpression = (ViewState["SORTEXPRESSION"] == null) ? null : (ViewState["SORTEXPRESSION"].ToString());
            int? sortdirection = 1;

            if (ViewState["SORTDIRECTION"] != null)
                sortdirection = Int32.Parse(ViewState["SORTDIRECTION"].ToString());

            DataTable dt = PhoenixCrewOffshoreLetterOfIntent.LetterOfIntentHistorySearch(Int32.Parse(ViewState["EMPID"].ToString())
                                                                                     , sortexpression, sortdirection,
                                                                                      (int)ViewState["PAGENUMBER"],
                                                                                      gvLetterOfInt.PageSize,
                                                                                      ref iRowCount,
                                                                                      ref iTotalPageCount);
            gvLetterOfInt.DataSource = dt;
            gvLetterOfInt.VirtualItemCount = iRowCount;

            ViewState["ROWCOUNT"] = iRowCount;
            ViewState["TOTALPAGECOUNT"] = iTotalPageCount;
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }
    protected void MenuIntSummary_TabStripCommand(object sender, EventArgs e)
    {
        try
        {
            RadToolBarEventArgs dce = (RadToolBarEventArgs)e;
            string CommandName = ((RadToolBarButton)dce.Item).CommandName;

            if (CommandName.ToUpper().Equals("EXCEL"))
            {
                ShowExcel();
            }
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }
    protected void ShowExcel()
    {
        int iRowCount = 0;
        int iTotalPageCount = 0;

        string[] alColumns = { };
        string[] alCaptions = { };

        //if (Filter.CurrentCrewLaunchedFrom != null && Filter.CurrentCrewLaunchedFrom.ToString() != "")
        //{
        alColumns = new string[] { "FLDFILENO", "FLDNAME", "FLDRANKNAME", "FLDVESSELTYPESNAME", "FLDSALAGREEDUSD", "FLDREJOININGBONUS", "FLDLETTEROFINTENTSTATUS" };
        alCaptions = new string[] { "Employee Number", "Name", "Rank", "Vessel Type", "Daily Rate", "Rejoining Bouns", "Status" };
        //}

        string sortexpression;
        int? sortdirection = null;

        sortexpression = (ViewState["SORTEXPRESSION"] == null) ? null : (ViewState["SORTEXPRESSION"].ToString());
        if (ViewState["SORTDIRECTION"] != null)
            sortdirection = Int32.Parse(ViewState["SORTDIRECTION"].ToString());
        if (ViewState["ROWCOUNT"] == null || Int32.Parse(ViewState["ROWCOUNT"].ToString()) == 0)
            iRowCount = 10;
        else
            iRowCount = Int32.Parse(ViewState["ROWCOUNT"].ToString());

        DataTable dt = PhoenixCrewOffshoreLetterOfIntent.LetterOfIntentHistorySearch(Int32.Parse(ViewState["EMPID"].ToString())
                                                                                     , sortexpression, sortdirection,
                                                                                      (int)ViewState["PAGENUMBER"],
                                                                                      iRowCount,
                                                                                      ref iRowCount,
                                                                                      ref iTotalPageCount);
        Response.AddHeader("Content-Disposition", "attachment; filename=LetterOfIntentHistory.xls");
        Response.ContentType = "application/vnd.msexcel";
        Response.Write("<TABLE BORDER='0' CELLPADDING='2' CELLSPACING='0' width='100%'>");
        Response.Write("<tr>");
        Response.Write("<td><img src='http://" + Request.Url.Authority + Session["images"] + "/esmlogo4_small.png" + "' /></td>");
        Response.Write("<td><h3>Assessment</h3></td>");
        Response.Write("<td colspan='" + (alColumns.Length - 2).ToString() + "'>&nbsp;</td>");
        Response.Write("</tr>");
        Response.Write("</TABLE>");
        Response.Write("<br />");
        Response.Write("<TABLE BORDER='1' CELLPADDING='2' CELLSPACING='2' width='100%'>");
        Response.Write("<tr>");
        for (int i = 0; i < alCaptions.Length; i++)
        {
            Response.Write("<td width='20%'>");
            Response.Write("<b>" + alCaptions[i] + "</b>");
            Response.Write("</td>");
        }
        Response.Write("</tr>");
        foreach (DataRow dr in dt.Rows)
        {
            Response.Write("<tr>");
            for (int i = 0; i < alColumns.Length; i++)
            {
                Response.Write("<td>");
                Response.Write(dr[alColumns[i]]);
                Response.Write("</td>");
            }
            Response.Write("</tr>");
        }
        Response.Write("</TABLE>");
        Response.End();
    }
    protected void cmdSort_Click(object sender, EventArgs e)
    {
        try
        {
            ImageButton ib = (ImageButton)sender;

            ViewState["SORTEXPRESSION"] = ib.CommandName;
            ViewState["SORTDIRECTION"] = int.Parse(ib.CommandArgument);
            BindData();

        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }
    protected void gvLetterOfInt_NeedDataSource(object sender, Telerik.Web.UI.GridNeedDataSourceEventArgs e)
    {
        try
        {
            ViewState["PAGENUMBER"] = ViewState["PAGENUMBER"] != null ? ViewState["PAGENUMBER"] : gvLetterOfInt.CurrentPageIndex + 1;
            BindData();
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }
    protected void gvLetterOfInt_ItemDataBound(object sender, GridItemEventArgs e)
    {
        if (e.Item is GridDataItem)
        {
            LinkButton lblFLDISSUEDDATELOI = (LinkButton)e.Item.FindControl("lblFLDISSUEDDATELOI");

            RadLabel lblEmployeeid = (RadLabel)e.Item.FindControl("lblEmployeeid");
            RadLabel lblloiletterid = (RadLabel)e.Item.FindControl("lblloiletterid");
            LinkButton cmdOfferLetter = (LinkButton)e.Item.FindControl("cmdOfferLetter");
            RadLabel lblstatusvalue = (RadLabel)e.Item.FindControl("lblstatusvalue");
            if (lblstatusvalue != null && lblstatusvalue.Text == "0")
            {
                //cmdOfferLetter.Attributes.Add("onclick", "javascript:$modalWindow.modalWindowUrl = '../CrewOffshore/CrewOffshoreLetterOfIntent.aspx?empid=" + lblEmployeeid.Text + "&letterid=" + lblloiletterid.Text + "&launchedfrom=offshore';showDialog('LOI');");

                //HtmlGenericControl html = new HtmlGenericControl();
                //html.InnerHtml = "<span class=\"icon\" ><i class=\"fas fa-appletter_black\"></i></span>";
                //cmdOfferLetter.Controls.Add(html);
                String scriptpopup = String.Format("javascript:openNewWindow('codehelp2','','" + Session["sitepath"] + "/CrewOffshore/CrewOffshoreLeterOfIntentMail.aspx?letterid=" + lblloiletterid.Text + "');");
                ScriptManager.RegisterStartupScript(Page, Page.GetType(), "", scriptpopup, true);
            }
            if (lblstatusvalue != null && lblstatusvalue.Text == "1")
            {
                //cmdOfferLetter.Attributes.Add("onclick", "javascript:$modalWindow.modalWindowUrl = '../CrewOffshore/CrewOffshoreLetterOfIntent.aspx?empid=" + lblEmployeeid.Text + "&letterid=" + lblloiletterid.Text + "&launchedfrom=offshore';showDialog('LOI');");

                //HtmlGenericControl html = new HtmlGenericControl();
                //html.InnerHtml = "<span class=\"icon\" ><i class=\"fas fa-appletter_yellow\"></i></span>";
                //cmdOfferLetter.Controls.Add(html);

                //cmdOfferLetter.Attributes.Add("onclick", "javascript:openNewWindow('codehelp2','','" + Session["sitepath"] + "/CrewOffshore/CrewOffshoreLeterOfIntentMail.aspx?letterid=" + lblloiletterid.Text + "');");
                cmdOfferLetter.Attributes.Add("onclick", "javascript:openNewWindow('codehelp2','','" + Session["sitepath"] + "/Options/OptionCrewOffshoreLetterofIntentEmail.aspx?letterid=" + lblloiletterid.Text + "');");

                HtmlGenericControl html = new HtmlGenericControl();
                html.InnerHtml = "<span class=\"icon\" ><i class=\"fas fa-appletter_yellow\"></i></span>";
                cmdOfferLetter.Controls.Add(html);
                
                //String scriptpopup = String.Format("javascript:openNewWindow('codehelp2','','" + Session["sitepath"] + "/CrewOffshore/CrewOffshoreLeterOfIntentMail.aspx?letterid=" + lblloiletterid.Text + "');");
                //ScriptManager.RegisterStartupScript(Page, Page.GetType(), "", scriptpopup, true);

            }
            if (lblstatusvalue != null && lblstatusvalue.Text == "2")
            {
                cmdOfferLetter.Attributes.Add("onclick", "javascript:$modalWindow.modalWindowUrl = '../CrewOffshore/CrewOffshoreLetterofIntentAcceptedDetails.aspx?empid=" + lblEmployeeid.Text + "&letterid=" + lblloiletterid.Text + "&launchedfrom=offshore';showDialog('LOI');");

                HtmlGenericControl html = new HtmlGenericControl();
                html.InnerHtml = "<span class=\"icon\" ><i class=\"fas fa-appletter_green\"></i></span>";
                cmdOfferLetter.Controls.Add(html);
            }
        }
    }
    protected void gvLetterOfInt_ItemCommand(object sender, GridCommandEventArgs e)
    {
        try
        {
            if (e.CommandName.ToUpper().Equals("DELETE"))
            {
                string lblloiletterid = ((RadLabel)e.Item.FindControl("lblloiletterid")).Text;

                PhoenixCrewOffshoreLetterOfIntent.LetterOfIntentCancel(General.GetNullableGuid(lblloiletterid), "letter of intent not sent but canceled");
                BindData();
                gvLetterOfInt.Rebind();
            }

            if (e.CommandName == "Page")
            {
                ViewState["PAGENUMBER"] = null;
            }
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }

    protected void HomeMenu_TabStripCommand(object sender, EventArgs e)
    {
        try
        {
            RadToolBarEventArgs dce = (RadToolBarEventArgs)e;
            string CommandName = ((RadToolBarButton)dce.Item).CommandName;

            if (CommandName.ToUpper().Equals("HOME"))
            {
                ScriptManager.RegisterClientScriptBlock(this, this.GetType(),
                           "BookMarkScript", "fnReloadList('codehelp1', 'ifMoreInfo', null);", true);
            }
            if (CommandName.ToUpper().Equals("PLAN"))
            {
                Response.Redirect("../Portal/PortalSeafarerPlanProposed.aspx?empid=" + ViewState["EMPID"], false);
            }
            if (CommandName.ToUpper().Equals("LETTEROFINTENT"))
            {
                Response.Redirect("../Portal/PortalSeafarerPlanLetterofIntent.aspx?empid=" + ViewState["EMPID"], false);
            }
            if (CommandName.ToUpper().Equals("OFFERLETTER"))
            {
                int iRowCount = 0;
                int iTotalPageCount = 0;
                string CrewPlanID = null;
                string sortexpression = (ViewState["SORTEXPRESSION"] == null) ? null : (ViewState["SORTEXPRESSION"].ToString());
                int? sortdirection = null;
                if (ViewState["SORTDIRECTION"] != null)
                    sortdirection = Int32.Parse(ViewState["SORTDIRECTION"].ToString());

                DataSet ds = PhoenixPortalSeafarer.PortalSearchCrewPlan(Convert.ToInt32(ViewState["EMPID"].ToString()), sortexpression, sortdirection,
                                                                                          (int)ViewState["PAGENUMBER"],
                                                                                          General.ShowRecords(null),
                                                                                          ref iRowCount,
                                                                                          ref iTotalPageCount);
                if (ds.Tables[0].Rows.Count > 0)
                {
                    CrewPlanID = ds.Tables[0].Rows[0]["FLDCREWPLANID"].ToString();
                    Response.Redirect("../Options/OptionCrewOffshoreOfferLetter.aspx?Crewplanid=" + CrewPlanID + "&empid=" + ViewState["EMPID"] + "&portal=1", false);
                }
                else
                {
                    ucError.ErrorMessage = "No Offer Letter";
                    ucError.Visible = true;
                    return;
                }

                ViewState["ROWCOUNT"] = iRowCount;
                ViewState["TOTALPAGECOUNT"] = iTotalPageCount;

               
            }
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }

}