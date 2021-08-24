using SouthNests.Phoenix.Common;
using SouthNests.Phoenix.Framework;
using SouthNests.Phoenix.Registers;
using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Data;
using System.Drawing;
using System.Web;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using Telerik.Web.UI;

public partial class Inspection_InspectionRegulation : PhoenixBasePage
{
    protected void Page_Load(object sender, EventArgs e)
    {
        SessionUtil.PageAccessRights(this.ViewState);
        cmdHiddenSubmit.Attributes.Add("style", "display:none");
        ShowToolBar();
        if (IsPostBack == false)
        {
            ViewState["PAGENUMBER"] = 1;
            ViewState["SORTEXPRESSION"] = null;
            ViewState["SORTDIRECTION"] = null;
            gvNewRegulations.PageSize = int.Parse(PhoenixGeneralSettings.CurrentGeneralSetting.Records);
        }
    }

    protected void cmdHiddenSubmit_Click(object sender, EventArgs e)
    {
        try
        {
            gvNewRegulations.Rebind();
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }
    public void ShowToolBar()
    {
        PhoenixToolbar toolbarmain = new PhoenixToolbar();
        toolbarmain = new PhoenixToolbar();
        toolbarmain.AddFontAwesomeButton("../Inspection/InspectionRegulation.aspx", "Export to Excel", "<i class=\"fas fa-file-excel\"></i>", "EXCEL");
        toolbarmain.AddFontAwesomeButton("javascript:CallPrint('gvNewRegulations')", "Print", "<i class=\"fas fa-print\"></i>", "PRINT");
        //toolbarmain.AddFontAwesomeButton("javascript:openNewWindow('MoreInfo','','" + Session["sitepath"] + "/Inspection/InspectionRegulationAdd.aspx'); return false;", "Add New Regulation", "<i class=\"fa fa-plus-circle\"></i>", "ADD");
        gvTabStrip.MenuList = toolbarmain.Show();
    }


    protected void gvTabStrip_TabStripCommand(object sender, EventArgs e)
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
    private bool IsValidReport(string IssuedDate, string IssuedBy, string Title, string Description, string ActionRequired)
    {
        bool validatePass = true;
        ucError.HeaderMessage = "Please provide the following required information";
        if (string.IsNullOrWhiteSpace(IssuedDate))
        {
            ucError.ErrorMessage = "Issued Date Cannot be Empty";
            validatePass = false;
        }

        if (string.IsNullOrWhiteSpace(IssuedBy))
        {
            ucError.ErrorMessage = "Issued By Cannot be Empty";
            validatePass = false;
        }

        if (string.IsNullOrWhiteSpace(Title))
        {
            ucError.ErrorMessage = "Title Cannot be Empty";
            validatePass = false;
        }

        if (string.IsNullOrWhiteSpace(Description))
        {
            ucError.ErrorMessage = "Description Cannot be Empty";
            validatePass = false;
        }

        if (string.IsNullOrWhiteSpace(ActionRequired))
        {
            ucError.ErrorMessage = "Action Required Cannot be Empty";
            validatePass = false;
        }

        return validatePass;
    }

    private string GetOverDueColor(int dateDiff)
    {
        string color = "";
        if (dateDiff <= 60 && dateDiff >= 30)
        {
            color = "#00e601";
        }
        else if (dateDiff < 30)
        {
            color = "#ffc200";
        }
        return color;
    }

    private string GetOverDueToolTip(int dateDiff)
    {
        string tooltip = "";
        if (dateDiff <= 60 && dateDiff >= 30)
        {
            tooltip = "Due on 60 days or less";
        }
        else if (dateDiff < 30)
        {
            tooltip = "Due on 30 days or less";
        }
        return tooltip;
    }

    #region Grid Releated Events
    public DataSet BindData(ref int iRowCount, ref int iTotalPageCount)
    {
        DataSet ds = new DataSet();
        bool isVessel = false;
        if (PhoenixSecurityContext.CurrentSecurityContext.InstallCode > 0) isVessel = true;
        
        ds = PhoenixInspectionNewRegulation.RegulationList((int)ViewState["PAGENUMBER"], gvNewRegulations.PageSize, ref iRowCount, ref iTotalPageCount, isVessel);
        return ds;
    }
    protected void gvNewRegulations_NeedDataSource(object sender, EventArgs e)
    {
        int iRowCount = 0;
        int iTotalPageCount = 0;
        ViewState["PAGENUMBER"] = ViewState["PAGENUMBER"] != null ? ViewState["PAGENUMBER"] : gvNewRegulations.CurrentPageIndex + 1;
        DataSet ds = BindData(ref iRowCount, ref iTotalPageCount);
        gvNewRegulations.DataSource = ds;
        gvNewRegulations.VirtualItemCount = iRowCount;
        PrintReport(ds);
    }
    protected void gvNewRegulations_ItemDataBound(object sender, GridItemEventArgs e)
    {
        if (e.Item.IsInEditMode)
        {
            RadLabel lblVesselType = (RadLabel)e.Item.FindControl("lblVesselTypeEdit");
            UserControlVesselTypeList ddlVesselTypeEdit = (UserControlVesselTypeList)e.Item.FindControl("chkvesselListEdit");
            if (ddlVesselTypeEdit != null)
            {
                ddlVesselTypeEdit.VesselTypeList = PhoenixRegistersVesselType.ListVesselType(0);
                if (ddlVesselTypeEdit.SelectedVesseltype == "")
                    ddlVesselTypeEdit.SelectedVesseltype = lblVesselType.Text;
            }
        }
        if (e.Item is GridEditableItem)
        {
            LinkButton db = (LinkButton)e.Item.FindControl("cmdDelete");
            if (db != null)
            {
                db.Attributes.Add("onclick", "return fnConfirmDelete(event); return false;");
                if (!SessionUtil.CanAccess(this.ViewState, db.CommandName)) db.Visible = false;
            }
            LinkButton sb = (LinkButton)e.Item.FindControl("cmdSave");
            if (sb != null) sb.Visible = SessionUtil.CanAccess(this.ViewState, sb.CommandName);

            LinkButton cb = (LinkButton)e.Item.FindControl("cmdCancel");
            if (cb != null) cb.Visible = SessionUtil.CanAccess(this.ViewState, cb.CommandName);
        }
        if (e.Item is GridDataItem && e.Item.IsInEditMode == false)
        {
            LinkButton attachmentBtn = (LinkButton)e.Item.FindControl("cmdAttachment");
            if (attachmentBtn != null) attachmentBtn.Visible = SessionUtil.CanAccess(this.ViewState, attachmentBtn.CommandName);
            GridDataItem item = (GridDataItem)e.Item;
            RadLabel dtKey = (RadLabel)item.FindControl("lbldtkey");
            DataRowView drv = (DataRowView)e.Item.DataItem;
            if (attachmentBtn!= null && dtKey != null)
            {
                if (drv["FLDISATTACHMENT"].ToString() == "0")
                {
                    HtmlGenericControl html = new HtmlGenericControl();
                    html.InnerHtml = "<span class=\"icon\" style=\"color:gray\"><i class=\"fas fa-paperclip\"></i></span>";
                    attachmentBtn.Controls.Add(html);
                }
                attachmentBtn.Attributes.Add("onclick", "javascript:openNewWindow('NAFA','','" + Session["sitepath"] + "/Common/CommonFileAttachment.aspx?dtkey=" + dtKey.Text + "&mod=QUALITY'); return false;");
            }

            GridDataItem dataItem = (GridDataItem)e.Item;
            TableCell cell = dataItem["Duedate"]; //where dataItem is object of type GridDataItem
            RadLabel lblDueDate = (RadLabel)dataItem.FindControl("lblDuedate");
            DateTime duedate = DateTime.Parse(lblDueDate.Text);
            DateTime today = DateTime.Now;
            TimeSpan dateDiff = today.Subtract(duedate);
            cell.Attributes.Add("style", "background-color:" + GetOverDueColor(Math.Abs(dateDiff.Days)));
            cell.ToolTip = GetOverDueToolTip(Math.Abs(dateDiff.Days));

            LinkButton complianceStatusBtn = (LinkButton)e.Item.FindControl("cmdComplianceStatus");
            if (complianceStatusBtn != null) complianceStatusBtn.Visible = SessionUtil.CanAccess(this.ViewState, complianceStatusBtn.CommandName);

            LinkButton delBtn = (LinkButton)e.Item.FindControl("cmdDelete");
            if (delBtn != null && PhoenixSecurityContext.CurrentSecurityContext.InstallCode > 0)
            {
                delBtn.Visible = false;
            }
        }
    }
    protected void gvNewRegulations_ItemCommand(object sender, GridCommandEventArgs e)
    {
        try
        {
            if (e.CommandName.ToUpper().Equals("SELECT"))
            {
                GridDataItem item = (GridDataItem)e.Item;
                RadLabel RegulationId = (RadLabel)item.FindControl("lblRegulationId");
                RadLabel RegulationStatus = (RadLabel)item.FindControl("lblStatus");
                Response.Redirect("../Inspection/InspectionRegulationAdd.aspx?&RegulationId=" + RegulationId.Text + "&LaunchFrom=1", true);
                //string url = string.Format("../Inspection/InspectionRegulationRule.aspx?RegulationId={0}&Status={1}", new Guid(RegulationId.Text), RegulationStatus.Text);
                //Response.Redirect(url);
            }
            if (e.CommandName.ToUpper().Equals("DELETE"))
            {
                LinkButton db1 = (LinkButton)e.Item.FindControl("cmdDelete");
                PhoenixInspectionNewRegulation.RegulationDelete(new Guid(db1.CommandArgument), PhoenixSecurityContext.CurrentSecurityContext.UserCode);
                gvNewRegulations.Rebind();
            }
            if (e.CommandName == "VIEW")
            {
                RadLabel RegulationId = (RadLabel)e.Item.FindControl("lblRegulationID");
                Response.Redirect("../Inspection/InspectionRegulationComplianceStatus.aspx?RegulationId=" + RegulationId.Text, false);
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
    #endregion
    private string[] getAlColumns()
    {
        string[] alColumns = { "FLDISSUEDATE", "FLDISSUEDBYNAME", "FLDTITLE", "FLDDUEDATE", "FLDVESSELTYPE", "FLDDESCRIPTION", "FLDACTIONREQUIRED", "FLDREMARKS" };
        return alColumns;
    }
    private string[] getAlCaptions()
    {
        string[] alCaptions = { "Issued Date", "Issued By", "Title", "Duedate", "Vessel Type", "Description", "Action Required", "Remarks" };
        return alCaptions;
    }
    private void PrintReport(DataSet ds)
    {
        string[] alColumns = getAlColumns();
        string[] alCaptions = getAlCaptions();
        General.SetPrintOptions("gvNewRegulations", "New Regulation", alCaptions, alColumns, ds);
    }
    private void ShowExcel()
    {
        int iRowCount = 0;
        int iTotalPageCount = 0;

        DataSet ds = new DataSet();
        string[] alColumns = getAlColumns();
        string[] alCaptions = getAlCaptions();
        string sortexpression;
        int? sortdirection = null;

        sortexpression = (ViewState["SORTEXPRESSION"] == null) ? null : (ViewState["SORTEXPRESSION"].ToString());
        if (ViewState["SORTDIRECTION"] != null)
            sortdirection = Int32.Parse(ViewState["SORTDIRECTION"].ToString());
        if (ViewState["ROWCOUNT"] == null || Int32.Parse(ViewState["ROWCOUNT"].ToString()) == 0)
            iRowCount = 10;
        else
            iRowCount = Int32.Parse(ViewState["ROWCOUNT"].ToString());

        gvNewRegulations.PageSize = int.Parse(PhoenixGeneralSettings.CurrentGeneralSetting.Records);
        ds = BindData(ref iRowCount, ref iTotalPageCount);
        Response.AddHeader("Content-Disposition", "attachment; filename=NewRegulation.xls");
        Response.ContentType = "application/vnd.msexcel";
        Response.Write("<TABLE BORDER='0' CELLPADDING='2' CELLSPACING='0' width='100%'>");
        Response.Write("<tr>");
        Response.Write("<td><img src='http://" + Request.Url.Authority + Session["images"] + "/esmlogo4_small.png" + "' /></td>");
        Response.Write("<td><h3>New Regulation</h3></td>");
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
        foreach (DataRow dr in ds.Tables[0].Rows)
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

}