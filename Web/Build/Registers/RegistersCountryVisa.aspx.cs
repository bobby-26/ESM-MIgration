using System;
using System.Data;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using SouthNests.Phoenix.Framework;
using SouthNests.Phoenix.Registers;
using SouthNests.Phoenix.Common;
using System.Text.RegularExpressions;
using Telerik.Web.UI;

public partial class RegisterCountryVisa : PhoenixBasePage
{

    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {
            SessionUtil.PageAccessRights(this.ViewState);
            PhoenixToolbar toolbar = new PhoenixToolbar();
            toolbar.AddFontAwesomeButton("../Registers/RegistersCountryVisa.aspx", "Export to Excel", "<i class=\"fas fa-file-excel\"></i>", "Excel");
            toolbar.AddFontAwesomeButton("javascript:CallPrint('gvCountryVisa')", "Print Grid", "<i class=\"fas fa-print\"></i>", "PRINT");
            toolbar.AddFontAwesomeButton("../Registers/RegistersCountryVisa.aspx", "Find", "<i class=\"fas fa-search\"></i>", "FIND");
            toolbar.AddFontAwesomeButton("javascript:openNewWindow('codehelp1','','" + Session["sitepath"] + "/Registers/RegistersCountryVisaAdd.aspx?VisaID=&countryid=&VisaType=&TimeTaken= &OnArrival=&DaysRequired=&PhysPres=&PhyPreSpec=&UrgentProcedure=&Passport=&Remarks=&OrdinaryAmount=&UrgentAmount=');", "New Visa", "<i class=\"fa fa-plus-circle\"></i>", "ADD");            
            MenuRegistersCountryVisa.AccessRights = this.ViewState;
            MenuRegistersCountryVisa.MenuList = toolbar.Show();            

            if (!IsPostBack)
            {
                cmdHiddenSubmit.Attributes.Add("style", "display:none");
                ViewState["PAGENUMBER"] = 1;
                ViewState["SORTEXPRESSION"] = null;
                ViewState["SORTDIRECTION"] = null;
                gvCountryVisa.PageSize = int.Parse(PhoenixGeneralSettings.CurrentGeneralSetting.Records);
            }

            if (Request.QueryString["countryid"] != null)
                ucCountry.SelectedCountry = Request.QueryString["countryid"].ToString();
            else
                ViewState["countryid"] = "";
            toolbar = new PhoenixToolbar();
            MenuTitle.AccessRights = this.ViewState;

            MenuTitle.MenuList = toolbar.Show();

        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }

    protected void RegistersCountryVisa_TabStripCommand(object sender, EventArgs e)
    {
        try
        {
            RadToolBarEventArgs dce = (RadToolBarEventArgs)e;
            string CommandName = ((RadToolBarButton)dce.Item).CommandName;
            if (CommandName.ToUpper().Equals("EXCEL"))
            {
                ShowExcel();
            }
            if (CommandName.ToUpper().Equals("FIND"))
            {
                BindData();
                gvCountryVisa.Rebind();
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

        string[] alColumns = { "FLDCOUNTRYNAME", "FLDVISATYPENAME", "FLDTIMETAKEN", "FLDDAYSREQUIREDFORVISA", "FLDPHYSICALPRESENCEYESNO", "FLDPHYSICALPRESENCESPECIFICATION", "FLDURGENTPROCEDURE", "FLDORDINARYAMOUNT", "FLDURGENTAMOUNT", "FLDREMARKS", "FLDMODIFIEDBYNAME", "FLDMODIFIEDDATE" };
        string[] alCaptions = { "Country Name", "Visa Type", "Time Taken", "Days Required", "Physical Presence Y/N", "Physical Presence Specification", "Urgent Procedure", "Ordinary Amount(USD)", "Urgent Amount(USD)", "Remarks", "Last Modified By", "Modified Date" };

        string sortexpression = (ViewState["SORTEXPRESSION"] == null) ? null : (ViewState["SORTEXPRESSION"].ToString());
        int? sortdirection = null;
        if (ViewState["SORTDIRECTION"] != null)
            sortdirection = Int32.Parse(ViewState["SORTDIRECTION"].ToString());

        DataSet ds = PhoenixRegistersCountryVisa.CountryVisaSearch(General.GetNullableInteger(ucCountry.SelectedCountry), General.GetNullableInteger(ucType.SelectedHard),
                                        sortexpression, sortdirection, (int)ViewState["PAGENUMBER"], gvCountryVisa.PageSize, ref iRowCount, ref iTotalPageCount);

        General.SetPrintOptions("gvCountryVisa", "CountryVisa", alCaptions, alColumns, ds);

        if (ds.Tables[0].Rows.Count > 0)
        {
            gvCountryVisa.DataSource = ds;
            gvCountryVisa.VirtualItemCount = iRowCount;
        }
        else
        {
            gvCountryVisa.DataSource = "";
        }
        //EnableDisablePhysicalPresenceSpecForEdit();
    }
   
    private void EnableDisablePhysicalPresenceSpecForEdit()
    {
        foreach (GridViewRow gvrow in gvCountryVisa.Items)
        {
            CheckBox CheckBox1 = (CheckBox)gvrow.FindControl("chkPhysicalPresenceYNEdit");
            if (CheckBox1 != null)
            {
                if (CheckBox1.Checked)
                {
                    ((TextBox)gvrow.FindControl("txtPhysicalPresenceSpecificationEdit")).Visible = true;
                }
                else
                {
                    ((TextBox)gvrow.FindControl("txtPhysicalPresenceSpecificationEdit")).Visible = false;
                }
            }
        }
    }

    private void SetKeyDownScroll(object sender, GridViewRowEventArgs e)
    {
        int nextRow = 0;
        GridView _gridView = (GridView)sender;

        if (e.Row.RowType == DataControlRowType.DataRow
            && (e.Row.RowState == (DataControlRowState.Edit | DataControlRowState.Selected | DataControlRowState.Alternate)
            || e.Row.RowState == (DataControlRowState.Edit | DataControlRowState.Selected | DataControlRowState.Normal)))
        {
            int nRows = ((DataSet)_gridView.DataSource).Tables[0].Rows.Count - 1;

            String script = "var keyValue = SelectSibling(event); ";
            script += " if(keyValue == 38) {";  //Up Arrow
            nextRow = (e.Row.RowIndex == 0) ? nRows : e.Row.RowIndex - 1;

            script += Page.ClientScript.GetPostBackEventReference(_gridView, "Edit$" + (nextRow).ToString(), false);
            script += "}";

            script += " if(keyValue == 40) {";  //Down Arrow
            nextRow = (e.Row.RowIndex == nRows) ? 0 : e.Row.RowIndex + 1;

            script += Page.ClientScript.GetPostBackEventReference(_gridView, "Edit$" + (nextRow).ToString(), false);
            script += "}";
            script += " if(keyValue != 40 && keyValue != 38) { eleminscroll.focus(); return true; } return false;";
            e.Row.Attributes["onkeydown"] = script;
        }

    }

    private bool IsValidCountryVisa(
        string countryname, string visatype
        , string timetaken, string daysrequired
        , int physicalpresence, string physicalpresencespecification
        , string urgentprocedure)
    {
        ucError.HeaderMessage = "Please provide the following required information";

        if (General.GetNullableInteger(countryname) == null)
            ucError.ErrorMessage = "Country Name is required.";

        if (General.GetNullableInteger(visatype) == null)
            ucError.ErrorMessage = "Visa type is required.";

        if (timetaken.Trim().Equals(""))
            ucError.ErrorMessage = "Time taken is required.";

        if (daysrequired.Trim().Equals(""))
            ucError.ErrorMessage = "Days required is required.";

        if (physicalpresence == 1)
            if (physicalpresencespecification == string.Empty)
                ucError.ErrorMessage = "Physical presence specification is required";

        if (urgentprocedure.Trim().Equals(""))
            ucError.ErrorMessage = "Urgent procedure is required.";

        return (!ucError.IsError);
    }

    protected void ShowExcel()
    {
        int iRowCount = 0;
        int iTotalPageCount = 0;

        DataSet ds = new DataSet();

        string[] alColumns = { "FLDCOUNTRYNAME", "FLDVISATYPENAME", "FLDTIMETAKEN", "FLDDAYSREQUIREDFORVISA", "FLDPHYSICALPRESENCEYESNO", "FLDPHYSICALPRESENCESPECIFICATION", "FLDURGENTPROCEDURE", "FLDORDINARYAMOUNT", "FLDURGENTAMOUNT", "FLDREMARKS", "FLDMODIFIEDBYNAME", "FLDMODIFIEDDATE" };
        string[] alCaptions = { "Country Name", "Visa Type", "Time Taken", "Days Required", "Physical Presence Y/N", "Physical Presence Specification", "Urgent Procedure", "Ordinary Amount(USD)", "Urgent Amount(USD)", "Remarks", "Last Modified By", "Modified Date" };

        string sortexpression;
        int? sortdirection = null;

        sortexpression = (ViewState["SORTEXPRESSION"] == null) ? null : (ViewState["SORTEXPRESSION"].ToString());
        if (ViewState["SORTDIRECTION"] != null)
            sortdirection = Int32.Parse(ViewState["SORTDIRECTION"].ToString());

        if (ViewState["ROWCOUNT"] == null || Int32.Parse(ViewState["ROWCOUNT"].ToString()) == 0)
            iRowCount = 10;
        else
            iRowCount = Int32.Parse(ViewState["ROWCOUNT"].ToString());

        ds = PhoenixRegistersCountryVisa.CountryVisaSearch(General.GetNullableInteger(ucCountry.SelectedCountry), General.GetNullableInteger(ucType.SelectedHard),
                                sortexpression, sortdirection, (int)ViewState["PAGENUMBER"], gvCountryVisa.PageSize, ref iRowCount, ref iTotalPageCount);


        Response.AddHeader("Content-Disposition", "attachment; filename=CountryVisa.xls");
        Response.ContentType = "application/vnd.msexcel";
        Response.Write("<TABLE BORDER='0' CELLPADDING='2' CELLSPACING='0' width='100%'>");
        Response.Write("<tr>");
        Response.Write("<td><img src='http://" + Request.Url.Authority + Session["images"] + "/esmlogo4_small.png" + "' /></td>");
        Response.Write("<td><h3>Country Visa</h3></td>");
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

    private void DeleteCountryVisa(string visaid)
    {
        PhoenixRegistersCountryVisa.DeleteCountryVisa(
            PhoenixSecurityContext.CurrentSecurityContext.UserCode, int.Parse(visaid));
    }
    protected void cmdHiddenSubmit_Click(object sender, EventArgs e)
    {
        try
        {
            BindData();
            gvCountryVisa.Rebind();
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }

    protected void gvCountryVisa_NeedDataSource(object sender, Telerik.Web.UI.GridNeedDataSourceEventArgs e)
    {
        ViewState["PAGENUMBER"] = ViewState["PAGENUMBER"] != null ? ViewState["PAGENUMBER"] : gvCountryVisa.CurrentPageIndex + 1;
        BindData();
    }

    protected void gvCountryVisa_ItemCommand(object sender, Telerik.Web.UI.GridCommandEventArgs e)
    {
        try
        {
            if (e.CommandName.ToUpper().Equals("SORT"))
                return;
            else if (e.CommandName.ToUpper().Equals("DELETE"))
            {
                DeleteCountryVisa(((RadLabel)e.Item.FindControl("lblVisaID")).Text);
                BindData();
                gvCountryVisa.Rebind();
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

    protected void gvCountryVisa_ItemDataBound(object sender, Telerik.Web.UI.GridItemEventArgs e)
    {
        if (e.Item is GridDataItem)
        {
            DataRowView drv = (DataRowView)e.Item.DataItem;
            LinkButton del = (LinkButton)e.Item.FindControl("cmdDelete");
            if (del != null)
            {
                del.Visible = SessionUtil.CanAccess(this.ViewState, del.CommandName);
                del.Attributes.Add("onclick", "return fnConfirmDelete(event); return false;");
            }

            LinkButton eb = (LinkButton)e.Item.FindControl("cmdEdit");
            if (eb != null) eb.Visible = SessionUtil.CanAccess(this.ViewState, eb.CommandName);

            LinkButton sb = (LinkButton)e.Item.FindControl("cmdSave");
            if (sb != null) sb.Visible = SessionUtil.CanAccess(this.ViewState, sb.CommandName);

            LinkButton cb = (LinkButton)e.Item.FindControl("cmdCancel");
            if (cb != null) cb.Visible = SessionUtil.CanAccess(this.ViewState, cb.CommandName);

            RadLabel lblVisaId = (RadLabel)e.Item.FindControl("lblVisaID");
            RadLabel lblCountry = (RadLabel)e.Item.FindControl("lblCountryName");

            LinkButton cmdVisaDocument = (LinkButton)e.Item.FindControl("cmdVisaDocuments");

            if (cmdVisaDocument != null)
            {
                cmdVisaDocument.Visible = SessionUtil.CanAccess(this.ViewState, cmdVisaDocument.CommandName);
                cmdVisaDocument.Attributes.Add("onclick", "javascript:openNewWindow('AddVisaDocument'" + ",'Country Visa Document'" + ", '" + Session["sitepath"] + "/Registers/RegistersCountryVisaDocument.aspx?visaid=" + lblVisaId.Text + "&countryname=" + lblCountry.Text + "'); return false;");
            }

            LinkButton cmdSendMail = (LinkButton)e.Item.FindControl("cmdSendMail");

            if (cmdSendMail != null)
            {
                cmdSendMail.Visible = SessionUtil.CanAccess(this.ViewState, cmdSendMail.CommandName);
                cmdSendMail.Attributes.Add("onclick", "javascript:openNewWindow('codehelp1','','" + Session["sitepath"] + "/Registers/RegistersCountryVisaEmail.aspx?visaid=" + lblVisaId.Text + "'); return false;");
            }

            LinkButton cmdExcel = (LinkButton)e.Item.FindControl("cmdExcel");

            if (cmdExcel != null)
            {
                cmdExcel.Visible = SessionUtil.CanAccess(this.ViewState, cmdExcel.CommandName);
                cmdExcel.Attributes.Add("onclick", "javascript:openNewWindow('codehelp1','','" + Session["sitepath"] + "/Registers/RegistersExport2XL.aspx?visaid=" + lblVisaId.Text + "'); return false;");
            }
            LinkButton img = (LinkButton)e.Item.FindControl("imgRemarks");

            if (img != null)
            {
                img.Attributes.Add("onclick", "javascript:openNewWindow('codehelp1','', '" + Session["sitepath"] + "/Registers/RegistersCountryVisaRemarks.aspx?id=" + lblVisaId.Text + "', 'xlarge')");
                img.Visible = General.GetNullableInteger(drv["FLDREMARKSCOUNT"].ToString()) == 0 ? false : true;
            }
            LinkButton imgNoRemarks = (LinkButton)e.Item.FindControl("imgNoRemarks");

            if (imgNoRemarks != null)
            {
                imgNoRemarks.Attributes.Add("onclick", "javascript:openNewWindow('codehelp1','', '" + Session["sitepath"] + "/Registers/RegistersCountryVisaRemarks.aspx?id=" + lblVisaId.Text + "', 'xlarge')");
                imgNoRemarks.Visible = General.GetNullableInteger(drv["FLDREMARKSCOUNT"].ToString()) == 0 ? true : false;
            }

            UserControlCountry ucCountry = (UserControlCountry)e.Item.FindControl("ddlCountryEdit");

            if (ucCountry != null) ucCountry.SelectedCountry = drv["FLDCOUNTRYID"].ToString();

            UserControlHard ucHard = (UserControlHard)e.Item.FindControl("ddlVisaTypeEdit");
            DataRowView drvHard = (DataRowView)e.Item.DataItem;
            if (ucHard != null) ucHard.SelectedHard = drvHard["FLDVISATYPE"].ToString();

            RadLabel lblPhysicalPresenceSpecification = (RadLabel)e.Item.FindControl("lblPhysicalPresenceSpecification");
            UserControlToolTip ucPhyPresenceTT = (UserControlToolTip)e.Item.FindControl("ucPhyPresenceTT");
            if (lblPhysicalPresenceSpecification != null)
            {
                lblPhysicalPresenceSpecification.Attributes.Add("onmouseover", "showTooltip(ev, '" + ucPhyPresenceTT.ToolTip + "', 'visible');");
                lblPhysicalPresenceSpecification.Attributes.Add("onmouseout", "showTooltip(ev, '" + ucPhyPresenceTT.ToolTip + "', 'hidden');");
            }

            RadLabel lblUrgentProcedure = (RadLabel)e.Item.FindControl("lblUrgentProcedure");
            UserControlToolTip ucUrgentProcTT = (UserControlToolTip)e.Item.FindControl("ucUrgentProcTT");
            if (lblUrgentProcedure != null)
            {
                lblUrgentProcedure.Attributes.Add("onmouseover", "showTooltip(ev, '" + ucUrgentProcTT.ToolTip + "', 'visible');");
                lblUrgentProcedure.Attributes.Add("onmouseout", "showTooltip(ev, '" + ucUrgentProcTT.ToolTip + "', 'hidden');");
            }


            Guid? lblDtkey = General.GetNullableGuid(drv["FLDDTKEY"].ToString());
            LinkButton cmdAttachment = (LinkButton)e.Item.FindControl("cmdAttachment");
            if (cmdAttachment != null)
            {
                cmdAttachment.Attributes.Add("onclick", "javascript:openNewWindow('codehelp1','','" + Session["sitepath"] + "/Common/CommonFileAttachment.aspx?dtkey=" + lblDtkey + "&mod="
                                    + PhoenixModule.REGISTERS + "');return true;");
                cmdAttachment.Visible = General.GetNullableInteger(drv["FLDATTACHMENTCOUNT"].ToString()) == 0 ? false : true;
            }
            LinkButton cmdNoAttachment = (LinkButton)e.Item.FindControl("cmdNoAttachment");
            if (cmdNoAttachment != null)
            {
                cmdNoAttachment.Attributes.Add("onclick", "javascript:openNewWindow('codehelp1','','" + Session["sitepath"] + "/Common/CommonFileAttachment.aspx?dtkey=" + lblDtkey + "&mod="
                                    + PhoenixModule.REGISTERS + "');return true;");
                cmdNoAttachment.Visible = General.GetNullableInteger(drv["FLDATTACHMENTCOUNT"].ToString()) == 0 ? true : false;
            }
            LinkButton cmdEdit = (LinkButton)e.Item.FindControl("cmdEdit");

            if (cmdEdit != null)
            {
                string sRemarks = Regex.Replace(((RadLabel)e.Item.FindControl("lblRemarks")).Text, "<.*?>", String.Empty);
                cmdEdit.Attributes.Add("onclick", "pjavascript:openNewWindow('codehelp1', '', '" + Session["sitepath"] + "/Registers/RegistersCountryVisaAdd.aspx?VisaID=" + ((RadLabel)e.Item.FindControl("lblVisaID")).Text +
                    "&countryid=" + ((RadLabel)e.Item.FindControl("lblCountryID")).Text +
                    "&VisaType=" + ((RadLabel)e.Item.FindControl("lblVisaTypeID")).Text +
                    "&TimeTaken=" + ((RadLabel)e.Item.FindControl("lblTimeTaken")).Text +
                    "&OnArrival=" + ((RadLabel)e.Item.FindControl("lblOnArrivalID")).Text +
                    "&DaysRequired=" + ((RadLabel)e.Item.FindControl("lblDaysRequried")).Text +
                    "&PhysPres=" + ((RadLabel)e.Item.FindControl("lblPhysicalPresenceYNID")).Text +
                    "&PhyPreSpec=" + ((RadLabel)e.Item.FindControl("lblPhyPresenceTT")).Text +
                    "&UrgentProcedure=" + ((RadLabel)e.Item.FindControl("lblUrgentProcedureText")).Text +
                    "&Passport=" + ((RadLabel)e.Item.FindControl("lblPassportID")).Text +
                    "&Remarks=  &OrdinaryAmount=" + ((RadLabel)e.Item.FindControl("lblOrdinaryAmount")).Text +
                    "&UrgentAmount=" + ((RadLabel)e.Item.FindControl("lblUrgentAmount")).Text +
                    "');return true;");
            }
        }
    }

    protected void gvCountryVisa_SortCommand(object sender, GridSortCommandEventArgs e)
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
}
           
