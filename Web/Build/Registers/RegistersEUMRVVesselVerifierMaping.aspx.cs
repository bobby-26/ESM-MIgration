using System;
using System.Data;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using SouthNests.Phoenix.Framework;
using SouthNests.Phoenix.Registers;
using Telerik.Web.UI;
public partial class RegistersEUMRVVesselVerifierMaping : PhoenixBasePage
{
    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {
            SessionUtil.PageAccessRights(this.ViewState);

            PhoenixToolbar toolbar = new PhoenixToolbar();
            toolbar.AddFontAwesomeButton("../Registers/RegistersEUMRVVesselVerifierMaping.aspx", "Export to Excel", "<i class=\"fas fa-file-excel\"></i>", "Excel");
            toolbar.AddFontAwesomeButton("javascript:CallPrint('gvVesselVerifierMap')", "Print Grid", "<i class=\"fas fa-print\"></i>", "PRINT");
            MenuVesselVerifierMap.AccessRights = this.ViewState;
            MenuVesselVerifierMap.MenuList = toolbar.Show();

            if (!IsPostBack)
            {
                ViewState["PAGENUMBER"] = 1;
                ViewState["SORTEXPRESSION"] = null;
                ViewState["SORTDIRECTION"] = null;
                gvVesselVerifierMap.PageSize = int.Parse(PhoenixGeneralSettings.CurrentGeneralSetting.Records);
            }
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }

    protected void VesselVerifierMap_TabStripCommand(object sender, EventArgs e)
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
    protected void Rebind()
    {
        gvVesselVerifierMap.SelectedIndexes.Clear();
        gvVesselVerifierMap.EditIndexes.Clear();
        gvVesselVerifierMap.DataSource = null;
        gvVesselVerifierMap.Rebind();
    }
    protected void gvVesselVerifierMap_ItemCommand(object sender, GridCommandEventArgs e)
    {
        try
        {
            //DataRowView drv = (DataRowView)e.Row.DataItem;
            if (e.CommandName.ToUpper().Equals("ADD"))
            {
                string tomail = ((RadTextBox)e.Item.FindControl("txtToMailAdd")).Text;
                string ccmail = ((RadTextBox)e.Item.FindControl("txtCCMailAdd")).Text;
                string ROtomail = ((RadTextBox)e.Item.FindControl("txtMPAToMailAdd")).Text;
                string ROccmail = ((RadTextBox)e.Item.FindControl("txtMPACCMailAdd")).Text;

                PhoenixRegistersEUMRVVesselVerifierMaping.EUMRVVesselVerifierInsert(
                        General.GetNullableInteger(((UserControlVessel)e.Item.FindControl("UcVessel")).SelectedVessel),
                        General.GetNullableGuid(((RadDropDownList)e.Item.FindControl("ddlVerifierAdd")).SelectedValue),
                        General.GetNullableString(((RadDropDownList)e.Item.FindControl("ddlDataFormatAdd")).SelectedValue),
                        General.GetNullableString(tomail),
                        General.GetNullableString(ccmail),
                        General.GetNullableString(ROtomail),
                        General.GetNullableString(ROccmail),
                        General.GetNullableGuid(((RadDropDownList)e.Item.FindControl("ddlROfficerAdd")).SelectedValue)

                        );
                Rebind();
            }
            else if (e.CommandName.ToUpper().Equals("DELETE"))
            {
                PhoenixRegistersEUMRVVesselVerifierMaping.EUMRVVesselVerifierDelete(
                    General.GetNullableGuid(((RadLabel)e.Item.FindControl("lblVesselVerifierMapid")).Text));
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

    protected void gvVesselVerifierMap_ItemDataBound(Object sender, GridItemEventArgs e)
    {
        try
        {
            if (e.Item is GridEditableItem)
            {
                DataRowView drv = (DataRowView)e.Item.DataItem;

                LinkButton edit = (LinkButton)e.Item.FindControl("cmdEdit");
                if (edit != null) edit.Visible = SessionUtil.CanAccess(this.ViewState, edit.CommandName);

                LinkButton del = (LinkButton)e.Item.FindControl("cmdDelete");
                if (del != null) del.Visible = SessionUtil.CanAccess(this.ViewState, del.CommandName);

                LinkButton save = (LinkButton)e.Item.FindControl("cmdSave");
                if (save != null) save.Visible = SessionUtil.CanAccess(this.ViewState, save.CommandName);

                LinkButton cancel = (LinkButton)e.Item.FindControl("cmdCancel");
                if (cancel != null) cancel.Visible = SessionUtil.CanAccess(this.ViewState, cancel.CommandName);

                LinkButton db = (LinkButton)e.Item.FindControl("cmdDelete");
                if (db != null) db.Attributes.Add("onclick", "return fnConfirmDelete(event); return false;");

                RadDropDownList ddlVerifierEdit = (RadDropDownList)e.Item.FindControl("ddlVerifierEdit");
                if (ddlVerifierEdit != null)
                {
                    ddlVerifierEdit.Items.Insert(0, new DropDownListItem("--Select--", "Dummy"));
                    ddlVerifierEdit.SelectedValue = drv["FLDEUMRVVERIFIERID"].ToString();
                }
                RadDropDownList ddlDataFormatEdit = (RadDropDownList)e.Item.FindControl("ddlDataFormatEdit");
                if (ddlDataFormatEdit != null)
                    ddlDataFormatEdit.SelectedValue = drv["FLDDATAFORMAT"].ToString();

                RadDropDownList ddlROfficerEdit = (RadDropDownList)e.Item.FindControl("ddlROfficerEdit"); 
                if (ddlROfficerEdit != null)
                {
                    ddlROfficerEdit.Items.Insert(0, new DropDownListItem("--Select--", "Dummy"));
                    ddlROfficerEdit.SelectedValue = drv["FLDROFFICER"].ToString();
                }
            }
            if (e.Item is GridDataItem)
            {
                RadLabel lbto = (RadLabel)e.Item.FindControl("lblToMail");
                UserControlToolTip ucto = (UserControlToolTip)e.Item.FindControl("ucToMail");
                if (lbto != null)
                {
                    ucto.Position = ToolTipPosition.TopCenter;
                    ucto.TargetControlId = lbto.ClientID;
                }

                RadLabel lbtcc = (RadLabel)e.Item.FindControl("lblCCMail");
                UserControlToolTip uctcc = (UserControlToolTip)e.Item.FindControl("ucCCMail");
                if (lbtcc != null)
                {
                    uctcc.Position = ToolTipPosition.TopCenter;
                    uctcc.TargetControlId = lbtcc.ClientID;
                }

                RadLabel lbtmo = (RadLabel)e.Item.FindControl("lblMPAToMail");
                UserControlToolTip uctmo = (UserControlToolTip)e.Item.FindControl("ucMPAToMail");
                if (lbtmo != null)
                {
                    uctmo.Position = ToolTipPosition.TopCenter;
                    uctmo.TargetControlId = lbtmo.ClientID;
                }

                RadLabel lbtmcc = (RadLabel)e.Item.FindControl("lblMPACCMail");
                UserControlToolTip uctmcc = (UserControlToolTip)e.Item.FindControl("ucMMPACCMail");
                if (lbtmcc != null)
                {
                    uctmcc.Position = ToolTipPosition.TopCenter;
                    uctmcc.TargetControlId = lbtmcc.ClientID;
                }
            }
            if (e.Item is GridFooterItem)
            {
                LinkButton db = (LinkButton)e.Item.FindControl("cmdAdd");
                if (db != null)
                {
                    if (!SessionUtil.CanAccess(this.ViewState, db.CommandName))
                        db.Visible = false;
                }
                RadDropDownList ddlVerifierAdd = (RadDropDownList)e.Item.FindControl("ddlVerifierAdd");
                if (ddlVerifierAdd != null)
                {
                    ddlVerifierAdd.Items.Insert(0, new DropDownListItem("--Select--", "Dummy"));
                }
                RadDropDownList ddlROfficerAdd = (RadDropDownList)e.Item.FindControl("ddlROfficerAdd");
                if (ddlROfficerAdd != null)
                {
                    ddlROfficerAdd.Items.Insert(0, new DropDownListItem("--Select--", "Dummy"));
                }
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
        int iRowCount = 0;
        int iTotalPageCount = 0;

        string[] alColumns = { "FLDVESSELNAME", "FLDVERIFIERNAME", "FLDDATAFORMAT", "FLDTOMAIL", "FLDCCMAIL" ,"FLDROFFICER", "FLDMPATOMAIL", "FLDMPACCMAIL" };
        string[] alCaptions = { "Vessel", "Verifier","Data Format","To Mail","CC Mail", "Recognized Organisation", "To Mail", "CC Mail" };

        string sortexpression = (ViewState["SORTEXPRESSION"] == null) ? null : (ViewState["SORTEXPRESSION"].ToString());
        int? sortdirection = null;

        if (ViewState["SORTDIRECTION"] != null)
            sortdirection = Int32.Parse(ViewState["SORTDIRECTION"].ToString());
       
        DataSet ds = PhoenixRegistersEUMRVVesselVerifierMaping.EUMRVVesselVerifierMapingSearch(null, sortexpression, sortdirection, (int)ViewState["PAGENUMBER"],
            gvVesselVerifierMap.PageSize,
            ref iRowCount,
            ref iTotalPageCount, null, null, null, null,null);

        General.SetPrintOptions("gvVesselVerifierMap", "RO Config", alCaptions, alColumns, ds);

        gvVesselVerifierMap.DataSource = ds;
        gvVesselVerifierMap.VirtualItemCount = iRowCount;
        
        ViewState["ROWCOUNT"] = iRowCount;
    }

    protected void ShowExcel()
    {
        int iRowCount = 0;
        int iTotalPageCount = 0;

        string[] alColumns = { "FLDVESSELNAME", "FLDVERIFIERNAME", "FLDDATAFORMAT", "FLDTOMAIL", "FLDCCMAIL", "FLDROFFICER", "FLDMPATOMAIL", "FLDMPACCMAIL" };
        string[] alCaptions = { "Vessel", "Verifier", "Data Format", "To Mail", "CC Mail", "Recognized Organisation", "To Mail", "CC Mail" };

        string sortexpression;
        int? sortdirection = null;

        sortexpression = (ViewState["SORTEXPRESSION"] == null) ? null : (ViewState["SORTEXPRESSION"].ToString());

        if (ViewState["SORTDIRECTION"] != null)
            sortdirection = Int32.Parse(ViewState["SORTDIRECTION"].ToString());

        if (ViewState["ROWCOUNT"] == null || Int32.Parse(ViewState["ROWCOUNT"].ToString()) == 0)
            iRowCount = 10;
        else
            iRowCount = Int32.Parse(ViewState["ROWCOUNT"].ToString());

        DataSet ds = PhoenixRegistersEUMRVVesselVerifierMaping.EUMRVVesselVerifierMapingSearch(null, sortexpression, sortdirection, 1,
            iRowCount,
            ref iRowCount,
            ref iTotalPageCount, null, null, null, null,null);

        Response.AddHeader("Content-Disposition", "attachment; filename=\"ROConfig.xls\"");
        Response.ContentType = "application/vnd.msexcel";
        Response.Write("<TABLE BORDER='0' CELLPADDING='2' CELLSPACING='0' width='100%'>");
        Response.Write("<tr>");
        Response.Write("<td><img src='http://" + Request.Url.Authority + Session["images"] + "/esmlogo4_small.png" + "' /></td>");
        Response.Write("<td><h3>RO Config</h3></td>");
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

    protected void gvVesselVerifierMap_UpdateCommand(object sender, GridCommandEventArgs e)
    {
        try
        {
            string tomail = ((RadTextBox)e.Item.FindControl("txtToMailEdit")).Text;
            string ccmail = ((RadTextBox)e.Item.FindControl("txtCCMailEdit")).Text;
            string ROtomail = ((RadTextBox)e.Item.FindControl("txtMPAToMailEdit")).Text;
            string ROccmail = ((RadTextBox)e.Item.FindControl("txtMPACCMailEdit")).Text;

            PhoenixRegistersEUMRVVesselVerifierMaping.EUMRVVesselVerifierUpdate(
                General.GetNullableInteger(((RadLabel)e.Item.FindControl("lblVesselid")).Text),
                General.GetNullableGuid(((RadDropDownList)e.Item.FindControl("ddlVerifierEdit")).SelectedValue),
                General.GetNullableString(((RadDropDownList)e.Item.FindControl("ddlDataFormatEdit")).SelectedValue),
                General.GetNullableString(tomail),
                General.GetNullableString(ccmail),
                 General.GetNullableGuid(((RadLabel)e.Item.FindControl("lblVesselVerifierMapid")).Text),
                 General.GetNullableString(ROtomail),
                General.GetNullableString(ROccmail),
                General.GetNullableGuid(((RadDropDownList)e.Item.FindControl("ddlROfficerEdit")).SelectedValue)
            );

            Rebind();
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }

    protected void gvVesselVerifierMap_NeedDataSource(object sender, GridNeedDataSourceEventArgs e)
    {
        try
        {
            ViewState["PAGENUMBER"] = ViewState["PAGENUMBER"] != null ? ViewState["PAGENUMBER"] : gvVesselVerifierMap.CurrentPageIndex + 1;
            BindData();
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }
}
