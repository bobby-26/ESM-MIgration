using System;
using System.Data;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using SouthNests.Phoenix.Framework;
using SouthNests.Phoenix.VesselAccounts;
using System.Collections.Specialized;
using SouthNests.Phoenix.Common;
using Telerik.Web.UI;

public partial class VesselAccountsOnboardTraining : PhoenixBasePage
{
    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {
            SessionUtil.PageAccessRights(this.ViewState);
            PhoenixToolbar toolbargrid = new PhoenixToolbar();
            toolbargrid.AddFontAwesomeButton("../VesselAccounts/VesselAccountsOnboardTraining.aspx", "Export to Excel", "<i class=\"fas fa-file-excel\"></i>", "Excel");
            toolbargrid.AddFontAwesomeButton("javascript:CallPrint('gvOnboardTraining')", "Print Grid", "<i class=\"fas fa-print\"></i>", "PRINT");
            MenuOnboardTraining.AccessRights = this.ViewState;
            MenuOnboardTraining.MenuList = toolbargrid.Show();


            if (!IsPostBack)
            {
                cmdHiddenSubmit.Attributes.Add("style", "display:none");
                ViewState["ONBOARDTRAININGID"] = null;
                ViewState["CURRENTTAB"] = null;
                ViewState["PAGENUMBER"] = 1;
                ViewState["SORTEXPRESSION"] = null;
                ViewState["SORTDIRECTION"] = null;
                ViewState["CURRENTINDEX"] = 1;
                gvOnboardTraining.PageSize = int.Parse(PhoenixGeneralSettings.CurrentGeneralSetting.Records);
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
        try
        {
            int iRowCount = 0;
            int iTotalPageCount = 0;
            string[] alColumns = { "FLDTRAININGNAME", "FLDFROMDATE", "FLDTODATE", "FLDTRAINEDBY", "FLDREMARKS", "FLDUPDATEDBY", "FLDMODIFIEDDATE" };
            string[] alCaptions = { "Training", "From", "To", "Trained By", "Remarks", "Modified By", "Modified On" };
            string sortexpression;
            int? sortdirection = null;

            sortexpression = (ViewState["SORTEXPRESSION"] == null) ? null : (ViewState["SORTEXPRESSION"].ToString());
            if (ViewState["SORTDIRECTION"] != null)
                sortdirection = Int32.Parse(ViewState["SORTDIRECTION"].ToString());

            if (ViewState["ROWCOUNT"] == null || Int32.Parse(ViewState["ROWCOUNT"].ToString()) == 0)
                iRowCount = 10;
            else
                iRowCount = Int32.Parse(ViewState["ROWCOUNT"].ToString());

            NameValueCollection nvc = Filter.CurrentVesselOnboardTrainingFilter;
            DataSet ds = PhoenixVesselAccountsOnboardTraining.SearchOnboardTraining(PhoenixSecurityContext.CurrentSecurityContext.VesselID
                            , General.GetNullableDateTime(nvc != null ? nvc["txtFromDate"] : string.Empty)
                            , General.GetNullableDateTime(nvc != null ? nvc["txtToDate"] : string.Empty)
                            , sortexpression, sortdirection
                            , Convert.ToInt16(ViewState["PAGENUMBER"].ToString())
                            , General.ShowRecords(null), ref iRowCount, ref iTotalPageCount);

            General.ShowExcel("Onboard Training", ds.Tables[0], alColumns, alCaptions, sortdirection, sortexpression);

        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }

    protected void MenuOnboardTraining_TabStripCommand(object sender, EventArgs e)
    {
        try
        {

            RadToolBarEventArgs dce = (RadToolBarEventArgs)e;
            string CommandName = ((RadToolBarButton)dce.Item).CommandName;
            if (CommandName.ToUpper().Equals("FIND"))
            {
                ViewState["PAGENUMBER"] = 1;
                Rebind();
            }
            else if (CommandName.ToUpper().Equals("EXCEL"))
            {
                ShowExcel();
            }
            else if (CommandName.ToUpper().Equals("CLEAR"))
            {
                ViewState["ONBOARDTRAININGID"] = null;
                Filter.CurrentVesselOnboardTrainingFilter = null;
                Rebind();
            }
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }
    private void BindData()
    {
        try
        {
            int iRowCount = 0;
            int iTotalPageCount = 0;
            string[] alColumns = { "FLDTRAININGNAME", "FLDFROMDATE", "FLDTODATE", "FLDTRAINEDBY", "FLDREMARKS", "FLDUPDATEDBY", "FLDMODIFIEDDATE" };
            string[] alCaptions = { "Training", "From", "To", "Trained By", "Remarks", "Modified By", "Modified On" };
            string sortexpression = (ViewState["SORTEXPRESSION"] == null) ? null : (ViewState["SORTEXPRESSION"].ToString());
            int? sortdirection = null;
            if (ViewState["SORTDIRECTION"] != null)
                sortdirection = Int32.Parse(ViewState["SORTDIRECTION"].ToString());
            NameValueCollection nvc = Filter.CurrentVesselOnboardTrainingFilter;
            DataSet ds = PhoenixVesselAccountsOnboardTraining.SearchOnboardTraining(PhoenixSecurityContext.CurrentSecurityContext.VesselID
                            , General.GetNullableDateTime(nvc != null ? nvc["txtFromDate"] : string.Empty)
                            , General.GetNullableDateTime(nvc != null ? nvc["txtToDate"] : string.Empty)
                            , sortexpression, sortdirection
                            , Convert.ToInt16(ViewState["PAGENUMBER"].ToString())
                            , gvOnboardTraining.PageSize, ref iRowCount, ref iTotalPageCount);
            General.SetPrintOptions("gvOnboardTraining", "Onboard Training", alCaptions, alColumns, ds);
            gvOnboardTraining.DataSource = ds;
            if (ds.Tables[0].Rows.Count > 0)
            {
                if (ViewState["ONBOARDTRAININGID"] == null)
                    ViewState["ONBOARDTRAININGID"] = ds.Tables[0].Rows[0]["FLDVESSELONBOARDTRAININGID"].ToString();
                
                if (ViewState["CURRENTTAB"] == null) ViewState["CURRENTTAB"] = "../VesselAccounts/VesselAccountsOnboardTrainingGeneral.aspx";
                ifMoreInfo.Attributes["src"] = ViewState["CURRENTTAB"] + "?OnboardTrainingid=" + ViewState["ONBOARDTRAININGID"];
            }
            else
            {
                ViewState["CURRENTTAB"] = "../VesselAccounts/VesselAccountsOnboardTrainingGeneral.aspx";
                ifMoreInfo.Attributes["src"] = "../VesselAccounts/VesselAccountsOnboardTrainingGeneral.aspx";
            }
            gvOnboardTraining.VirtualItemCount = iRowCount;
            ViewState["ROWCOUNT"] = iRowCount;
            ViewState["TOTALPAGECOUNT"] = iTotalPageCount;
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }
    protected void Rebind()
    {
        gvOnboardTraining.SelectedIndexes.Clear();
        gvOnboardTraining.EditIndexes.Clear();
        gvOnboardTraining.DataSource = null;
        gvOnboardTraining.Rebind();
    }
    protected void gvOnboardTraining_ItemCommand(object sender, GridCommandEventArgs e)
    {
        try
        {
            if (e.CommandName.ToUpper() == "SELECT")
            {
                string orderid = ((RadLabel)e.Item.FindControl("lblOnboardTrainingId")).Text;
                ViewState["ONBOARDTRAININGID"] = orderid;
                GridDataItem item = (GridDataItem)gvOnboardTraining.SelectedItems[0];
                ViewState["CURRENTTAB"] = "../VesselAccounts/VesselAccountsOnboardTrainingGeneral.aspx";
                ifMoreInfo.Attributes["src"] = ViewState["CURRENTTAB"] + "?OnboardTrainingid=" + ViewState["ONBOARDTRAININGID"];
            }
            else if (e.CommandName == "Page")
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
    protected void gvOnboardTraining_ItemDataBound(Object sender, GridItemEventArgs e)
    {
        if (e.Item is GridEditableItem)
        {
            DataRowView drv = (DataRowView)e.Item.DataItem;
            LinkButton att = (LinkButton)e.Item.FindControl("cmdAtt");

            if (att != null)
            {
                att.Visible = SessionUtil.CanAccess(this.ViewState, att.CommandName);
                if (drv["FLDISATTACHMENT"].ToString() == "0")
                {
                    HtmlGenericControl html = new HtmlGenericControl();
                    html.InnerHtml = "<span class=\"icon\" style=\"color:gray\"><i class=\"fas fa-paperclip\"></i></span>";
                    att.Controls.Add(html);
                }
                att.Attributes.Add("onclick", "javascript:parent.Openpopup('att','','" + Session["sitepath"] + "/Common/CommonFileAttachment.aspx?dtkey=" + drv["FLDDTKEY"].ToString() + "&mod="
                      + PhoenixModule.VESSELACCOUNTS + "'); return false;");

            }
            RadLabel lbtn = (RadLabel)e.Item.FindControl("lblRemarks");
            UserControlToolTip uct = (UserControlToolTip)e.Item.FindControl("ucToolTipAddress");
            uct.Position = ToolTipPosition.TopCenter;
            uct.TargetControlId = lbtn.ClientID;


        }
    }
    protected void gvOnboardTraining_NeedDataSource(object sender, GridNeedDataSourceEventArgs e)
    {
        ViewState["PAGENUMBER"] = ViewState["PAGENUMBER"] != null ? ViewState["PAGENUMBER"] : gvOnboardTraining.CurrentPageIndex + 1;
        BindData();
    }
    protected void cmdSearch_Click(object sender, EventArgs e)
    {
        try
        {
            Rebind();
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }
    protected void cmdHiddenSubmit_Click(object sender, EventArgs e)
    {
        try
        {
            ViewState["ONBOARDTRAININGID"] = null;
            Rebind();
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }
}
