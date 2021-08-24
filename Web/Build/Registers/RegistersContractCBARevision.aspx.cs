using System;
using System.Data;
using System.Web.UI;
using System.Web.UI.WebControls;
using SouthNests.Phoenix.Framework;
using SouthNests.Phoenix.Registers;
using System.Collections;
using SouthNests.Phoenix.Common;
using System.Configuration;
using Telerik.Web.UI;
using System.Web.UI.HtmlControls;

public partial class RegistersContractCBARevision : PhoenixBasePage
{
    string Unionid = string.Empty;
    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {
            SessionUtil.PageAccessRights(this.ViewState);
            cmdHiddenSubmit.Attributes.Add("style", "display:none");


            PhoenixToolbar toolbar1 = new PhoenixToolbar();
           
            toolbar1 = new PhoenixToolbar();

            toolbar1.AddButton("Nationality", "NATIONALITY", ToolBarDirection.Right);
            toolbar1.AddButton("Vessel", "VESSEL", ToolBarDirection.Right);
            MenuCBA.MenuList = toolbar1.Show();
         //   MenuCBA.ClearSelection();
          
            PhoenixToolbar toolbar = new PhoenixToolbar();
            toolbar.AddFontAwesomeButton("../Registers/RegistersContractCBArevision.aspx", "Export to Excel", "<i class=\"fas fa-file-excel\"></i>", "Excel");
            toolbar.AddFontAwesomeButton("javascript:CallPrint('gvCBARevision')", "Print Grid", "<i class=\"fas fa-print\"></i>", "PRINT");
            MenuContract.AccessRights = this.ViewState;
            MenuContract.MenuList = toolbar.Show();

            if (!IsPostBack)
            {
                ViewState["PAGENUMBER"] = int.Parse(Request.QueryString["pagenumber"] == null ? "1" : Request.QueryString["pagenumber"].ToString());
                ViewState["SORTEXPRESSION"] = null;
                ViewState["SORTDIRECTION"] = null;
                ddlUnion.AddressList = PhoenixRegistersAddress.ListAddress("134");
                if (Request.QueryString["Union"] != null)
                    ddlUnion.SelectedAddress = Request.QueryString["Union"].ToString();
                gvCBArevision.PageSize = int.Parse(PhoenixGeneralSettings.CurrentGeneralSetting.Records);
            }

        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }
    protected void Contract_TabStripCommand(object sender, EventArgs e)
    {
        try
        {
            RadToolBarEventArgs dce = (RadToolBarEventArgs)e;
            string CommandName = ((RadToolBarButton)dce.Item).CommandName;
            if (CommandName.ToUpper().Equals("EXCEL"))
            {
                int iRowCount = 0;
                int iTotalPageCount = 0;
                string[] alColumns = { "FLDREVISIONNO", "FLDEFFECTIVEDATE", "FLDEXPIRYDATE", "FLDGOTHOURS", "FLDLEAVEONSATURDAYNAME" };
                string[] alCaptions = { "Revision No.", "Effective From", "Effective To", "Guaranteed OT Hours", "Saturday Working Hours" };
                string sortexpression = (ViewState["SORTEXPRESSION"] == null) ? null : (ViewState["SORTEXPRESSION"].ToString());

                if (ViewState["ROWCOUNT"] == null || Int32.Parse(ViewState["ROWCOUNT"].ToString()) == 0)
                    iRowCount = 10;
                else
                    iRowCount = Int32.Parse(ViewState["ROWCOUNT"].ToString());
                DataTable dt = PhoenixRegistersContract.SearchCBARevision(int.Parse((ddlUnion.SelectedAddress == null || ddlUnion.SelectedAddress == "" || ddlUnion.SelectedAddress == "Dummy") ? "0" : ddlUnion.SelectedAddress)
                                                                                        , sortexpression
                                                                                        , 1
                                                                                        , 1
                                                                                        , iRowCount
                                                                                        , ref iRowCount
                                                                                        , ref iTotalPageCount);

                Response.AddHeader("Content-Disposition", "attachment; filename=CBASubComponent.xls");
                Response.ContentType = "application/vnd.msexcel";
                Response.Write("<TABLE BORDER='0' CELLPADDING='2' CELLSPACING='0' width='100%'>");
                Response.Write("");
                Response.Write("<tr><td ><img src='http://" + Request.Url.Authority + Session["images"] + "/esmlogo4_small.png" + "' /></td><td colspan='" + (alColumns.Length - 1).ToString() + "'><b><h13>  Collective Bargaining Agreement</h13></b></td></tr>");
                Response.Write("<tr><td colspan='" + (alColumns.Length).ToString() + "'><h5>UNION                              -" + (dt.Rows.Count > 0 ? dt.Rows[0]["FLDUNIONNAME"].ToString() : "") + "</h5></td></tr>");
                Response.Write("</tr>");
                Response.Write("</TABLE>");
                Response.Write("<br />");
                Response.Write("<TABLE BORDER='1' CELLPADDING='2' CELLSPACING='2' width='100%'>");
                Response.Write("<tr>");
                for (int i = 0; i < alCaptions.Length; i++)
                {
                    Response.Write("<td>");
                    Response.Write("<b>" + alCaptions[i] + "</b>");
                    Response.Write("</td>");
                }
                Response.Write("</tr>");
                foreach (DataRow dr in dt.Rows)
                {
                    Response.Write("<tr>");
                    for (int i = 0; i < alColumns.Length; i++)
                    {
                        Response.Write(dr[alColumns[i]].GetType().Equals(typeof(string)) ? "<td  class='text'>" : "<td>");
                        Response.Write(dr[alColumns[i]].GetType().Equals(typeof(DateTime)) ? General.GetDateTimeToString(dr[alColumns[i]].ToString()) : dr[alColumns[i]].ToString());
                        Response.Write("</td>");
                    }
                    Response.Write("</tr>");
                }

                Response.Write("</TABLE>");
                Response.Write(ConfigurationManager.AppSettings["softwarename"].ToString());
                Response.End();
                General.ShowExcel("Collective Bargaining Agreement", dt, alColumns, alCaptions, null, string.Empty);
            }
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }
    protected void gvCBArevision_NeedDataSource(object sender, GridNeedDataSourceEventArgs e)
    {
        try
        {
            ViewState["PAGENUMBER"] = ViewState["PAGENUMBER"] != null ? ViewState["PAGENUMBER"] : gvCBArevision.CurrentPageIndex + 1;

            BindData();
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }
    protected void cmdHiddenSubmit_Click(object sender, EventArgs e)
    {
        BindData();
    }
    private void BindData()
    {
        try
        {
            int iRowCount = 0;
            int iTotalPageCount = 0;
            string[] alColumns = { "FLDUNIONNAME", "FLDREVISIONNO", "FLDEFFECTIVEDATE", "FLDEXPIRYDATE", "FLDGOTHOURS", "FLDLEAVEONSATURDAYNAME" };
            string[] alCaptions = { "Union", "Revision No.", "Effective From", "Effective To", "Guaranteed OT Hours", "Saturday Working Hours" };
            string sortexpression = (ViewState["SORTEXPRESSION"] == null) ? null : (ViewState["SORTEXPRESSION"].ToString());
            DataTable dt = PhoenixRegistersContract.SearchCBARevision(int.Parse((ddlUnion.SelectedAddress == null || ddlUnion.SelectedAddress == "" || ddlUnion.SelectedAddress == "Dummy") ? "0" : ddlUnion.SelectedAddress)
                                                                                    , sortexpression
                                                                                    , 1
                                                                                    , int.Parse(ViewState["PAGENUMBER"].ToString())
                                                                                    , gvCBArevision.PageSize
                                                                                    , ref iRowCount
                                                                                    , ref iTotalPageCount);
            DataSet ds = new DataSet();
            ds.Tables.Add(dt.Copy());
            General.SetPrintOptions("gvCBARevision", "Collective Bargaining Agreement", alCaptions, alColumns, ds);

            gvCBArevision.DataSource = dt;
            gvCBArevision.VirtualItemCount = iRowCount;
            ViewState["ROWCOUNT"] = iRowCount;
            ViewState["TOTALPAGECOUNT"] = iTotalPageCount;

        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }
    protected void gvCBArevision_SortCommand(object sender, GridSortCommandEventArgs e)
    {
        ViewState["SORTEXPRESSION"] = e.SortExpression;
        switch (e.NewSortOrder)
        {
            case GridSortOrder.Ascending:
                ViewState["SORTDIRECTION"] = "0";
                break;
            case GridSortOrder.Descending:
                ViewState["SORTDIRECTION"] = "1";
                break;
        }
    }


    protected void Rebind()
    {
        gvCBArevision.SelectedIndexes.Clear();
        gvCBArevision.EditIndexes.Clear();
        gvCBArevision.DataSource = null;
        gvCBArevision.Rebind();
    }
    protected void gvCBArevision_ItemCommand(object sender, GridCommandEventArgs e)
    {
        try
        {
            if (e.CommandName.ToUpper().Equals("COMPONENTDETAILS"))
            {
                string RevisionId = ((RadLabel)e.Item.FindControl("lblRevisionid")).Text.Trim();
                Response.Redirect("../Registers/RegistersContractCBA.aspx?RevisionId=" + RevisionId + "&pagenumber=" + ViewState["PAGENUMBER"].ToString());
            }
            if (e.CommandName.ToUpper().Equals("WAGEDETAILS"))
            {
                string RevisionId = ((RadLabel)e.Item.FindControl("lblRevisionid")).Text.Trim();
                Response.Redirect("../Registers/RegistersContractCBAWage.aspx?RevisionId=" + RevisionId + "&pagenumber=" + ViewState["PAGENUMBER"].ToString());
            }
            if (e.CommandName.ToUpper().Equals("ADD"))
            {
                string EffectiveFrom = ((UserControlDate)e.Item.FindControl("txtEffectiveFromAdd")).Text;
                string EffectiveTo = ((UserControlDate)e.Item.FindControl("txtEffectiveToAdd")).Text;
                string GuaranteedOTAdd = ((UserControlMaskNumber)e.Item.FindControl("txtGuaranteedOTAdd")).Text.Trim();
                string ExtraOTAdd = ((RadTextBox)e.Item.FindControl("txtExtraOTAdd")).Text.Trim();
                string LeaveonSaturdayAdd = ((RadDropDownList)e.Item.FindControl("ddlLeaveonSaturday")).SelectedValue.Trim();
                if (!IsValidRevision(ddlUnion.SelectedAddress, EffectiveFrom, EffectiveTo, GuaranteedOTAdd))
                {
                    ucError.Visible = true;
                    return;
                }
                PhoenixRegistersContract.InsertCBAContractRevision(int.Parse(ddlUnion.SelectedAddress), DateTime.Parse(EffectiveFrom)
                                                                   , DateTime.Parse(EffectiveTo), General.GetNullableInteger(GuaranteedOTAdd)
                                                                   , General.GetNullableInteger(ExtraOTAdd), General.GetNullableInteger(LeaveonSaturdayAdd));
                Rebind();
            }
            else if (e.CommandName.ToUpper().Equals("SAVE"))
            {
                string RevisionId = ((RadLabel)e.Item.FindControl("lblRevisionid")).Text.Trim();
                string EffectiveFrom = ((UserControlDate)e.Item.FindControl("txtEffectiveFromEdit")).Text;
                string EffectiveTo = ((UserControlDate)e.Item.FindControl("txtEffectiveToEdit")).Text;
                string GuaranteedOT = ((UserControlMaskNumber)e.Item.FindControl("txtGuaranteedOTEdit")).Text.Trim();
                string ExtraOTE = ((RadTextBox)e.Item.FindControl("txtExtraOTEdit")).Text.Trim();
                string LeaveonSaturday = ((RadDropDownList)e.Item.FindControl("ddlLeaveonSaturdayEdit")).SelectedValue.Trim();
                if (!IsValidRevision(ddlUnion.SelectedAddress, EffectiveFrom, EffectiveTo, GuaranteedOT))
                {
                    ucError.Visible = true;
                    return;
                }
                PhoenixRegistersContract.UpdateCBAContractRevision(int.Parse(ddlUnion.SelectedAddress), DateTime.Parse(EffectiveFrom)
                                                                   , DateTime.Parse(EffectiveTo), new Guid(RevisionId)
                                                                   , General.GetNullableInteger(GuaranteedOT), General.GetNullableInteger(ExtraOTE)
                                                                   , General.GetNullableInteger(LeaveonSaturday));
                Rebind();
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

    protected void gvCBArevision_ItemDataBound(Object sender, GridItemEventArgs e)
    {
        if (e.Item is GridEditableItem)
        {
            DataRowView drv = (DataRowView)e.Item.DataItem;
            LinkButton db = (LinkButton)e.Item.FindControl("cmdattachedimg");
            if (db != null)
                db.Visible = SessionUtil.CanAccess(this.ViewState, db.CommandName);
            LinkButton eb = (LinkButton)e.Item.FindControl("cmdEdit");
            if (eb != null) eb.Visible = SessionUtil.CanAccess(this.ViewState, eb.CommandName);

            LinkButton sb = (LinkButton)e.Item.FindControl("cmdComponentDetails");
            if (sb != null) sb.Visible = SessionUtil.CanAccess(this.ViewState, sb.CommandName);

            LinkButton cb = (LinkButton)e.Item.FindControl("cmdWageDetails");
            if (cb != null) cb.Visible = SessionUtil.CanAccess(this.ViewState, cb.CommandName);

            LinkButton imgClip = (LinkButton)e.Item.FindControl("cmdattachedimg");
            if (imgClip != null)
            {
                if (drv["FLDATTACHMENTYN"].ToString() == "0")
                {
                    HtmlGenericControl html = new HtmlGenericControl();
                    html.InnerHtml = "<span class=\"icon\" style=\"color:gray\"><i class=\"fas fa-paperclip\"></i></span>";
                    imgClip.Controls.Add(html);
                }

                imgClip.Attributes["onclick"] = "javascript:openNewWindow('UPLOAD','','" + Session["sitepath"] + "/Common/CommonFileAttachment.aspx?dtkey=" + drv["FLDDTKEY"].ToString() + "&mod=" + PhoenixModule.REGISTERS + "'); return false;";
            }
            RadDropDownList LeaveonSaturday = (RadDropDownList)e.Item.FindControl("ddlLeaveonSaturdayEdit");
            if (LeaveonSaturday != null) LeaveonSaturday.SelectedValue = drv["FLDLEAVEONSATURDAY"].ToString();

        }
        //if (e.Item.IsInEditMode)
        //{
        //    UserControlDepartmentType ucDeptartmentType = (UserControlDepartmentType)e.Item.FindControl("ucDeparmentTypeEdit");
        //    DataRowView drvDeptartmentType = (DataRowView)e.Item.DataItem;
        //    if (ucDeptartmentType != null) ucDeptartmentType.SelectedDepartmentType = DataBinder.Eval(e.Item.DataItem, "FLDDEPARTMENTTYPEID").ToString();
        //}
        if (e.Item is GridFooterItem)
        {
            LinkButton db = (LinkButton)e.Item.FindControl("cmdAdd");
            if (db != null)
            {
                if (!SessionUtil.CanAccess(this.ViewState, db.CommandName))
                    db.Visible = false;
            }
        }
    }
    protected void cmdSearch_Click(object sender, EventArgs e)
    {
        Rebind();
    }
    protected void ddlUnion_Changed(object sender, EventArgs e)
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
    private bool IsValidRevision(string union, string effectivedate, string expirydate, string GOT)
    {
        ucError.HeaderMessage = "Please provide the following required information";

        int resultInt, resultGTO;
        DateTime resultDate;
        if (!int.TryParse(union, out resultInt))
            ucError.ErrorMessage = "Union is required.";

        if (!General.GetNullableDateTime(effectivedate).HasValue)
            ucError.ErrorMessage = "Effective From is required.";

        if (!General.GetNullableDateTime(expirydate).HasValue)
            ucError.ErrorMessage = "Effective To is required.";
        else if (General.GetNullableDateTime(effectivedate).HasValue && DateTime.TryParse(expirydate, out resultDate) && DateTime.Compare(DateTime.Parse(effectivedate), resultDate) > 0)
        {
            ucError.ErrorMessage = "Effective To should be later than Effective From";
        }
        if (GOT != "")
            if (!int.TryParse(GOT, out resultGTO) || int.Parse(GOT) < 0)
                ucError.ErrorMessage = "Guaranteed OT Hours is Positive Integer Field.";

        return (!ucError.IsError);
    }
    public StateBag ReturnViewState()
    {
        return ViewState;
    }

    protected void MenuCBA_TabStripCommand(object sender, EventArgs e)
    {
        RadToolBarEventArgs dce = (RadToolBarEventArgs)e;
        string CommandName = ((RadToolBarButton)dce.Item).CommandName;
        try
        {

            if (CommandName.ToUpper().Equals("NATIONALITY"))
            {
                    Response.Redirect("RegistersContractCBARevisionNationalityMapping.aspx?");
            }
           else if (CommandName.ToUpper().Equals("VESSEL"))
            {
                Response.Redirect("RegistersContractCBAVesselMapping.aspx?");
            }
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }
}
