using System;
using System.Data;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using SouthNests.Phoenix.Framework;
using SouthNests.Phoenix.Registers;
using SouthNests.Phoenix.Inspection;
using System.Collections.Specialized;
using SouthNests.Phoenix.Common;
using Telerik.Web.UI;


public partial class Accounts_AccountsBankDownloadMappedTemplates : PhoenixBasePage
{
  
    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {
            SessionUtil.PageAccessRights(this.ViewState);
            PhoenixToolbar toolbar = new PhoenixToolbar();
            toolbar.AddImageButton("../Accounts/AccountsBankDownloadMappedTemplates.aspx", "Export to Excel", "icon_xls.png", "Excel");
            toolbar.AddImageLink("javascript:CallPrint('gvTemplate')", "Print Grid", "icon_print.png", "PRINT");
            MenuRegistersBank.AccessRights = this.ViewState;
            MenuRegistersBank.MenuList = toolbar.Show();
         //   MenuRegistersBank.SetTrigger(pnlBankEntry);

            if (!IsPostBack)
            {
                ViewState["PAGENUMBER"] = 1;
                ViewState["SORTEXPRESSION"] = null;
                ViewState["SORTDIRECTION"] = null;
                ViewState["CURRENTINDEX"] = 1;

                if (Request.QueryString["bankid"] != null || Request.QueryString["bankid"] != string.Empty)
                {
                    ViewState["BANKID"] = Request.QueryString["bankid"];
                }

            }
            BindData();
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
        string docname = "Bank Download Template Mapping";

        string[] alColumns = { "FLDHARDNAME", "FLDTEMPLATECODE" };
        string[] alCaptions = { "Payment Mode", "Template Name" };

        string sortexpression;
        int? sortdirection = null;

        sortexpression = (ViewState["SORTEXPRESSION"] == null) ? null : (ViewState["SORTEXPRESSION"].ToString());

        if (ViewState["SORTDIRECTION"] != null)
            sortdirection = Int32.Parse(ViewState["SORTDIRECTION"].ToString());

        if (ViewState["ROWCOUNT"] == null || Int32.Parse(ViewState["ROWCOUNT"].ToString()) == 0)
            iRowCount = 10;
        else
            iRowCount = Int32.Parse(ViewState["ROWCOUNT"].ToString());

        DataSet ds = PhoenixAccountsRemittanceBankDownload.BankDownloadTemplateSearch(null, General.GetNullableInteger(ddltype.SelectedValue), null,
            General.GetNullableInteger(ViewState["BANKID"].ToString()),
            sortexpression, sortdirection,
            1,
            iRowCount,
            ref iRowCount,
            ref iTotalPageCount
            );

        Response.AddHeader("Content-Disposition", "attachment; filename=" + docname + ".xls");
        Response.ContentType = "application/vnd.msexcel";
        Response.Write("<TABLE BORDER='0' CELLPADDING='2' CELLSPACING='0' width='100%'>");
        Response.Write("<tr>");
        Response.Write("<td><img src='http://" + Request.Url.Authority + Session["images"] + "/esmlogo4_small.png" + "' /></td>");
        Response.Write("<td><h3>" + docname + "</h3></td>");
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

    protected void RegistersBank_TabStripCommand(object sender, EventArgs e)
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

    private void BindData()
    {
        int iRowCount = 0;
        int iTotalPageCount = 0;
        string docname = "";

        string[] alColumns = { "FLDHARDNAME", "FLDTEMPLATECODE" };
        string[] alCaptions = { "Payment Mode", "Template Name" };

        string sortexpression = (ViewState["SORTEXPRESSION"] == null) ? null : (ViewState["SORTEXPRESSION"].ToString());
        int? sortdirection = null;

        if (ViewState["SORTDIRECTION"] != null)
            sortdirection = Int32.Parse(ViewState["SORTDIRECTION"].ToString());


        DataSet ds = PhoenixAccountsRemittanceBankDownload.BankDownloadTemplateSearch(null, General.GetNullableInteger(ddltype.SelectedValue), null,
            General.GetNullableInteger(ViewState["BANKID"].ToString()),
            sortexpression, sortdirection,
            Int32.Parse(ViewState["PAGENUMBER"].ToString()),
            General.ShowRecords(null),
            ref iRowCount,
            ref iTotalPageCount
            );

        General.SetPrintOptions("gvTemplate", docname, alCaptions, alColumns, ds);

        //if (ds.Tables[0].Rows.Count > 0)
        //{
            gvTemplate.DataSource = ds;
            gvTemplate.VirtualItemCount=iRowCount;
        ViewState["ROWCOUNT"] = iRowCount;

        //}
        //else
        //{
        //    DataTable dt = ds.Tables[0];
        //    ShowNoRecordsFound(dt, gvTemplate);
        //}

        ViewState["TOTALPAGECOUNT"] = iTotalPageCount;
    }

    protected void gvTemplate_Sorting(object sender, GridSortCommandEventArgs e)
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

  

    protected void gvTemplate_RowCommand(object sender, GridCommandEventArgs e)
    {
        try
        {
            if (e.CommandName.ToUpper().Equals("SORT"))
                return;

        
            if (e.CommandName.ToUpper().Equals("ADD"))
            {


                if (!IsValidTemplate(((UserControlHard)e.Item.FindControl("ucpaymentmodeAdd")).SelectedHard, ((RadComboBox)e.Item.FindControl("ddltemplateadd")).SelectedValue))
                {
                    ucError.Visible = true;
                    return;
                }

                string bankid = ViewState["BANKID"].ToString();

                PhoenixAccountsRemittanceBankDownload.BankDownloadTemplateMappingInsert
                    (
                           General.GetNullableInteger(((UserControlHard)e.Item.FindControl("ucpaymentmodeAdd")).SelectedHard),
                           General.GetNullableInteger(bankid),
                           General.GetNullableGuid(((RadComboBox)e.Item.FindControl("ddltemplateadd")).SelectedValue.ToString())

                    );
                ucStatus.Text = "Information inserted";
                BindData();
                //((TextBox)_gridView.FooterRow.FindControl("txtInspectionNameAdd")).Focus();
            }
            else if (e.CommandName.ToUpper().Equals("DELETE"))
            {

                PhoenixAccountsRemittanceBankDownload.BankDownloadTemplateMappingDelete(new Guid(((RadLabel)e.Item.FindControl("lbltemplateMapId")).Text));

                BindData();
            }
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }


    private bool IsValidTemplate(string paymentmode, string template)
    {
        ucError.HeaderMessage = "Please provide the following required information";
        if (ddltype.SelectedValue == "0")
            ucError.ErrorMessage = "Type is required.";
        if (paymentmode.Trim().Equals("Dummy") || paymentmode.Trim().Equals(""))
            ucError.ErrorMessage = "Payment Mode is required.";
        if (template.Trim().Equals("Dummy") || template.Trim().Equals(""))
            ucError.ErrorMessage = "Template is required.";

        return (!ucError.IsError);
    }
    
    protected void gvTemplate_RowDeleting(object sender, GridCommandEventArgs de)
    {
        BindData();
    }

   

    protected void gvTemplate_RowUpdating(object sender, GridCommandEventArgs e)
    {
        try
        {
           
            if (!IsValidTemplate(
                     ((UserControlHard)e.Item.FindControl("ucpaymentmodeEdit")).SelectedHard
                , ((RadComboBox)e.Item.FindControl("ddltemplateedit")).SelectedValue))
            {
                ucError.Visible = true;
                return;
            }

            //Label lbltemplateedit = ((Label)_gridView.Rows[nCurrentRow].FindControl("lbltemplateMapIdEdit"));

            PhoenixAccountsRemittanceBankDownload.BankDownloadTemplateMappingUpdate
                (
                        new Guid(((RadLabel)e.Item.FindControl("lbltemplateMapIdEdit")).Text),
                       General.GetNullableInteger(((UserControlHard)e.Item.FindControl("ucpaymentmodeEdit")).SelectedHard),
                       General.GetNullableInteger(ViewState["BANKID"].ToString()),
                       General.GetNullableGuid(((RadComboBox)e.Item.FindControl("ddltemplateedit")).SelectedValue.ToString())

                );
            ucStatus.Text = "Information inserted";

            BindData();
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }

    protected void gvTemplate_SelectedIndexChanging(object sender, GridViewSelectEventArgs e)
    {
       // gvTemplate.SelectedIndex = e.NewSelectedIndex;
    }

    protected void gvTemplate_RowCreated(object sender, GridViewRowEventArgs e)
    {
        if (e.Row.RowType == DataControlRowType.DataRow
            && e.Row.RowState != (DataControlRowState.Edit | DataControlRowState.Selected | DataControlRowState.Alternate)
            && e.Row.RowState != (DataControlRowState.Edit | DataControlRowState.Selected | DataControlRowState.Normal))
        {
            e.Row.TabIndex = -1;
            e.Row.Attributes["onclick"] = Page.ClientScript.GetPostBackEventReference(gvTemplate, "Edit$" + e.Row.RowIndex.ToString(), false);
        }
    }
    protected void ddltype_SelectedIndexChanged(object sender, EventArgs e)
    {
        BindData();
    }

    protected void gvTemplate_ItemDataBound(Object sender, GridItemEventArgs e)
    {
       

        if (e.Item is GridDataItem)
        {
            DataRowView drv = (DataRowView)e.Item.DataItem;

            ImageButton db = (ImageButton)e.Item.FindControl("cmdDelete");
            if (db != null)
            {
                db.Attributes.Add("onclick", "return fnConfirmDelete(event); return false;");
                db.Visible = SessionUtil.CanAccess(this.ViewState, db.CommandName);
            }

            ImageButton eb = (ImageButton)e.Item.FindControl("cmdEdit");
            if (eb != null) eb.Visible = SessionUtil.CanAccess(this.ViewState, eb.CommandName);

            ImageButton sb = (ImageButton)e.Item.FindControl("cmdSave");
            if (sb != null) sb.Visible = SessionUtil.CanAccess(this.ViewState, sb.CommandName);

            ImageButton cb = (ImageButton)e.Item.FindControl("cmdCancel");
            if (cb != null) cb.Visible = SessionUtil.CanAccess(this.ViewState, cb.CommandName);

            RadComboBox ddltemplateedit = (RadComboBox)e.Item.FindControl("ddltemplateedit");
            if (ddltemplateedit != null)
            {
                ddltemplateedit.DataSource = PhoenixAccountsRemittanceBankDownload.BankTemplateList(General.GetNullableInteger(ddltype.SelectedValue));
                ddltemplateedit.DataTextField = "FLDTEMPLATECODE";
                ddltemplateedit.DataValueField = "FLDBANKDOWNLOADTEMPLATEID";
                ddltemplateedit.DataBind();
                ddltemplateedit.Items.Insert(0, new RadComboBoxItem("--Select--", "Dummy"));
            }

            if (ddltemplateedit != null)
            {
                ddltemplateedit.SelectedValue = drv["FLDBANKDOWNLOADTEMPLATEID"].ToString();
            }

            UserControlHard ucpaymentmodeEdit = (UserControlHard)e.Item.FindControl("ucpaymentmodeEdit");

            if (ucpaymentmodeEdit != null)
            {
                if (ddltype.SelectedValue == "1")
                    ucpaymentmodeEdit.HardList = PhoenixRegistersHard.ListHard(1, 132, 0, "CHQ,TT,ACH,MTT,MCH,ACHS,FT");
                else
                    ucpaymentmodeEdit.HardList = PhoenixRegistersHard.ListHard(1, 132, 0, "ATT,NLT,ALT");
                ucpaymentmodeEdit.DataBind();
                ucpaymentmodeEdit.SelectedHard = drv["FLDPAYMENTMODE"].ToString();
            }


        }
        if (e.Item is GridFooterItem)
        {
            ImageButton db = (ImageButton)e.Item.FindControl("cmdAdd");
            if (db != null)
            {
                if (!SessionUtil.CanAccess(this.ViewState, db.CommandName))
                    db.Visible = false;
            }
            UserControlHard ucpaymentmode = (UserControlHard)e.Item.FindControl("ucpaymentmodeAdd");
            
            if (ddltype.SelectedValue == "1")
                ucpaymentmode.HardList = PhoenixRegistersHard.ListHard(1, 132, 0, "CHQ,TT,ACH,MTT,MCH,ACHS,FT");
            else
                ucpaymentmode.HardList = PhoenixRegistersHard.ListHard(1, 132, 0, "ATT,NLT,ALT");
            ucpaymentmode.DataBind();
            RadComboBox ddltemplate = ((RadComboBox)e.Item.FindControl("ddltemplateadd"));
            if (ddltemplate != null)
            {
                ddltemplate.DataTextField = "FLDTEMPLATECODE";
                ddltemplate.DataValueField = "FLDBANKDOWNLOADTEMPLATEID";
                ddltemplate.DataSource = PhoenixAccountsRemittanceBankDownload.BankTemplateList(General.GetNullableInteger(ddltype.SelectedValue));
                ddltemplate.DataBind();
                ddltemplate.Items.Insert(0, new RadComboBoxItem("--Select--", "Dummy"));
            }

        }
    }

    //protected void CloseWindow(object sender, EventArgs e)
    //{
    //    try
    //    {
    //        string Script = "";
    //        Script += "fnReloadList(null,'ifMoreInfo','keepopen');";
    //        UserControlConfirmMessage ucCM = (UserControlConfirmMessage)sender;

    //        if (ucCM.confirmboxvalue == 1)
    //        {
    //            String script = String.Format("javascript:fnReloadList('codehelp1');");

    //            if (ViewState["InspectionId"] != null)
    //                CopyInspection(new Guid(ViewState["InspectionId"].ToString()), null, null);
    //            BindData();

    //            ucStatus.Text = "Schedule has been updated as 'Completed'";
    //            ScriptManager.RegisterStartupScript(Page, Page.GetType(), "script", script, true);
    //        }
    //        else
    //            BindData();
    //        SetPageNavigator();
    //    }
    //    catch (Exception ex)
    //    {
    //        ucError.ErrorMessage = ex.Message;
    //        ucError.Visible = true;
    //    }
    //}

   
    public StateBag ReturnViewState()
    {
        return ViewState;
    }

    protected void gvTemplate_NeedDataSource(object sender, Telerik.Web.UI.GridNeedDataSourceEventArgs e)
    {
        try
        {
            ViewState["PAGENUMBER"] = ViewState["PAGENUMBER"] != null ? ViewState["PAGENUMBER"] : gvTemplate.CurrentPageIndex + 1;
            BindData();
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }
}
