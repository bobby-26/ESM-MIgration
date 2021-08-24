using System;
using System.Collections;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Xml.Linq;
using SouthNests.Phoenix.Accounts;
using SouthNests.Phoenix.Framework;
using System.Collections.Specialized;
using SouthNests.Phoenix.Registers;
using Telerik.Web.UI;

public partial class AccountsProjectBillingItemByVessel : PhoenixBasePage
{
    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {
            SessionUtil.PageAccessRights(this.ViewState);

            PhoenixToolbar toolbargrid = new PhoenixToolbar();
            toolbargrid.AddImageButton("../Accounts/AccountsProjectBillingItemByVessel.aspx", "Export to Excel", "icon_xls.png", "Excel");
            toolbargrid.AddImageLink("javascript:CallPrint('gvFormDetails')", "Print Grid", "icon_print.png", "PRINT");


            MenuOrderForm.AccessRights = this.ViewState;
            MenuOrderForm.MenuList = toolbargrid.Show();
            //MenuOrderForm.SetTrigger(pnlOrderForm);

            PhoenixToolbar toolbar = new PhoenixToolbar();
            toolbar.AddButton("Vessel Specific", "VESSELSPECIFIC", ToolBarDirection.Right);
            toolbar.AddButton("General", "GENERAL", ToolBarDirection.Right);
            ProjectBilling.AccessRights = this.ViewState;
            ProjectBilling.Title = "Project Billing Item";
            ProjectBilling.MenuList = toolbar.Show();


            if (!IsPostBack)
            {
                cmdHiddenPick.Attributes.Add("style", "visibility:hidden;");


                if (Request.QueryString["projectbillingid"] != string.Empty)
                {
                    ViewState["projectbillingid"] = Request.QueryString["projectbillingid"];
                }
                ViewState["BUDGETID"] = null;
                ViewState["STOREITEMID"] = null;
                ProjectBillingEdit();
                ProjectBilling.SelectedMenuIndex = 0;

                ViewState["PAGENUMBER"] = 1;
                ViewState["SORTEXPRESSION"] = null;
                ViewState["SORTDIRECTION"] = null;
                gvFormDetails.PageSize = int.Parse(PhoenixGeneralSettings.CurrentGeneralSetting.Records);
            }

        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }
    protected void ProjectBillingEdit()
    {
        if (ViewState["projectbillingid"] != null)
        {

            DataSet ds = PhoenixAccountsProjectBilling.ProjectBillingEdit(new Guid(ViewState["projectbillingid"].ToString()));
            if (ds.Tables.Count > 0)
            {
                if (ds.Tables[0].Rows.Count > 0)
                {
                    DataRow dr = ds.Tables[0].Rows[0];

                    txtprojectBillingName.Text = dr["FLDPROJECTBILLINGNAME"].ToString();
                    txtBillingDescription.Text = dr["FLDBILLINGITEMDESCRIPTION"].ToString();
                    ucDefaultPrice.Text = dr["FLDAMOUNT"].ToString();
                    ViewState["BUDGETID"] = dr["FLDVESSELBUDGETID"].ToString();
                    ViewState["STOREITEMID"] = dr["FLDSTOREITEMID"].ToString();
                    txtNumber.Text = dr["FLDNUMBER"].ToString();
                    string temp3 = ViewState["BUDGETID"].ToString();

                }
            }
        }
    }
    protected void cmdHiddenSubmit_Click(object sender, EventArgs e)
    {

    }
    protected void ProjectBilling_TabStripCommand(object sender, EventArgs e)
    {
        try
        {
            RadToolBarEventArgs dce = (RadToolBarEventArgs)e;
            string CommandName = ((RadToolBarButton)dce.Item).CommandName;

            if (CommandName.ToUpper().Equals("GENERAL"))
            {
                Response.Redirect("../Accounts/AccountsProjectBillingMaster.aspx?projectbillingid=" + ViewState["projectbillingid"]);
            }
            if (CommandName.ToUpper().Equals("VESSELSPECIFIC") && ViewState["projectbillingid"] != null && ViewState["projectbillingid"].ToString() != string.Empty)
            {
                Response.Redirect("../Accounts/AccountsProjectBillingItemByVessel.aspx?projectbillingid=" + ViewState["projectbillingid"]);
            }
            else
                ProjectBilling.SelectedMenuIndex = 1;

        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }
    protected void MenuOrderForm_TabStripCommand(object sender, EventArgs e)
    {
        RadToolBarEventArgs dce = (RadToolBarEventArgs)e;
        string CommandName = ((RadToolBarButton)dce.Item).CommandName;
        try
        {
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
    protected void ShowExcel()
    {
        int iRowCount = 0;
        int iTotalPageCount = 0;

        DataSet ds = new DataSet();

        string[] alColumns = { "FLDACCOUNTCODE", "FLDDESCRIPTION", "FLDOWNERBUDGETGROUP", "FLDCOMPANYCODE", "FLDSTORENUMBER", "FLDSELLINGAMOUNT" };
        string[] alCaptions = { "Account Code", "Account Description", "Owner Budget Code", "Company", "Store Item", "Selling Price" };
        string sortexpression;
        int? sortdirection = null;

        sortexpression = (ViewState["SORTEXPRESSION"] == null) ? null : (ViewState["SORTEXPRESSION"].ToString());
        if (ViewState["SORTDIRECTION"] != null)
            sortdirection = Int32.Parse(ViewState["SORTDIRECTION"].ToString());

        if (ViewState["ROWCOUNT"] == null || Int32.Parse(ViewState["ROWCOUNT"].ToString()) == 0)
            iRowCount = 10;
        else
            iRowCount = Int32.Parse(ViewState["ROWCOUNT"].ToString());



        NameValueCollection nvc = Filter.CurrentSelectedProjectBillingItem;
        ds = PhoenixAccountsProjectBilling.ProjectBillingVesselMappingSearch(new Guid(ViewState["projectbillingid"].ToString())
                                                                                , null
                                                                                , null
                                                                                , 0
                                                                                , sortexpression
                                                                                , sortdirection
                                                                                , (int)ViewState["PAGENUMBER"]
                                                                                , General.ShowRecords(null)
                                                                                , ref iRowCount
                                                                                , ref iTotalPageCount);

        Response.AddHeader("Content-Disposition", "attachment; filename=ProjectBillingVesselMap.xls");
        Response.ContentType = "application/vnd.msexcel";
        Response.Write("<TABLE BORDER='0' CELLPADDING='2' CELLSPACING='0' width='100%'>");
        Response.Write("<tr>");
        Response.Write("<td><img src='http://" + Request.Url.Authority + Session["images"] + "/esmlogo4_small.png" + "' /></td>");
        Response.Write("<td><h3>Project Billing Inventory Map</h3></td>");
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
    private void BindData()
    {
        int iRowCount = 0;
        int iTotalPageCount = 0;
        DataSet ds = new DataSet();
        string sortexpression = (ViewState["SORTEXPRESSION"] == null) ? null : (ViewState["SORTEXPRESSION"].ToString());
        int? sortdirection = null;
        if (ViewState["SORTDIRECTION"] != null)
            sortdirection = Int32.Parse(ViewState["SORTDIRECTION"].ToString());

        NameValueCollection nvc = Filter.CurrentSelectedProjectBillingItem;
        ds = PhoenixAccountsProjectBilling.ProjectBillingVesselMappingSearch(new Guid(ViewState["projectbillingid"].ToString())
                                                                                , null
                                                                                , null
                                                                                , 0
                                                                                , sortexpression
                                                                                , sortdirection
                                                                                , gvFormDetails.CurrentPageIndex+1
                                                                                , gvFormDetails.PageSize
                                                                                , ref iRowCount
                                                                                , ref iTotalPageCount);

        string[] alColumns = { "FLDACCOUNTCODE", "FLDDESCRIPTION", "FLDOWNERBUDGETGROUP", "FLDCOMPANYCODE", "FLDSTORENUMBER", "FLDSELLINGAMOUNT" };
        string[] alCaptions = { "Account Code", "Account Description", "Owner Budget Code", "Company", "Store Item", "Selling Price" };

        General.SetPrintOptions("gvFormDetails", "Project Billing", alCaptions, alColumns, ds);


        gvFormDetails.DataSource = ds;
        gvFormDetails.VirtualItemCount = iRowCount;
        ViewState["ROWCOUNT"] = iRowCount;
        ViewState["TOTALPAGECOUNT"] = iTotalPageCount;
    }

    protected void gvFormDetails_ItemDataBound(Object sender, GridItemEventArgs e)
    {
        if (e.Item is GridEditableItem)
        {
            ImageButton cmdEdit = (ImageButton)e.Item.FindControl("cmdEdit");
            if (cmdEdit != null)
                if (!SessionUtil.CanAccess(this.ViewState, cmdEdit.CommandName)) cmdEdit.Visible = false;


            RadLabel lblvesselid = (RadLabel)e.Item.FindControl("lblVesselIdEdit");
            if (lblvesselid != null) lblvesselid.Attributes.Add("style", "visibility:hidden");

            RadTextBox tb1 = (RadTextBox)e.Item.FindControl("txtOwnerBudgetNameEdit");
            if (tb1 != null) tb1.Attributes.Add("style", "visibility:hidden");
            tb1 = (RadTextBox)e.Item.FindControl("txtOwnerBudgetIdEdit");
            if (tb1 != null) tb1.Attributes.Add("style", "visibility:hidden");
            tb1 = (RadTextBox)e.Item.FindControl("txtOwnerBudgetgroupIdEdit");
            if (tb1 != null) tb1.Attributes.Add("style", "visibility:hidden");
            tb1 = (RadTextBox)e.Item.FindControl("txtOwnerBudgetCodeEdit");
            ImageButton ibtnShowOwnerBudgetEdit = (ImageButton)e.Item.FindControl("btnShowOwnerBudgetEdit");
            UserControlCompany ucCompany = (UserControlCompany)e.Item.FindControl("ucCompany");

            RadLabel lblCompanyEdit = (RadLabel)e.Item.FindControl("lblCompanyEdit");
            if (ibtnShowOwnerBudgetEdit != null && lblvesselid != null)
            {
                ibtnShowOwnerBudgetEdit.Attributes.Add("onclick", "return showPickList('spnPickListOwnerBudgetEdit', 'codehelp1', '', '../Common/CommonPickListOwnerBudget.aspx?iframignore=true&vesselid=" + lblvesselid.Text + "&budgetdate=" + DateTime.Now.ToString() + "&budgetid=" + ViewState["BUDGETID"].ToString() + "', false); ");         //+ "&budgetid=" + lblbudgetid.Text       
                if (!SessionUtil.CanAccess(this.ViewState, ibtnShowOwnerBudgetEdit.CommandName)) ibtnShowOwnerBudgetEdit.Visible = false;
            }
            if (ucCompany != null)
            {
                //DataSet ds = PhoenixRegistersCompany.ListCompany();
                ucCompany.CompanyList = PhoenixRegistersCompany.ListCompany();
                ucCompany.DataBind();
                if (lblCompanyEdit != null)
                    ucCompany.SelectedCompany = lblCompanyEdit.Text;

            }
            RadTextBox tbItem2 = (RadTextBox)e.Item.FindControl("txtServiceNumber");
            if (tbItem2 != null) tbItem2.Attributes.Add("style", "visibility:hidden");
            RadTextBox tbItem3 = (RadTextBox)e.Item.FindControl("txtStoreItemId");
            if (tbItem3 != null) tbItem3.Attributes.Add("style", "visibility:hidden");

            ImageButton ib3 = (ImageButton)e.Item.FindControl("cmdShowItem");
            if (ib3 != null)
            {
                ib3.Attributes.Add("onclick", "return showPickList('spnPickItem', 'codehelp1', '', '../Common/CommonPickListStoreItem.aspx?iframignore=true&storetype=417&vesselid=" + lblvesselid.Text + "', fasle); ");

            }
        }

    }
    protected void gvFormDetails_ItemCommand(object sender, GridCommandEventArgs e)
    {
        try
        {
            if (e.CommandName == "Page")
            {
                ViewState["PAGENUMBER"] = null;
            }

            if (e.CommandName.ToUpper().Equals("SAVE"))
            {
                int iRowno = Int32.Parse(e.CommandArgument.ToString());

                PhoenixAccountsProjectBilling.ProjectBillingInventoryMapInsert(
                                                General.GetNullableGuid(((RadLabel)e.Item.FindControl("lblProjectBillingVesselInventoryMapIdEdit")).Text.ToString())
                                                , new Guid(ViewState["projectbillingid"].ToString())
                                                , int.Parse(((RadLabel)e.Item.FindControl("lblVesselIdEdit")).Text)
                                                , int.Parse(((RadLabel)e.Item.FindControl("lblAccountIdEdit")).Text)
                                                , General.GetNullableGuid(((RadTextBox)e.Item.FindControl("txtOwnerBudgetIdEdit")).Text.ToString())
                                                , General.GetNullableInteger(((UserControlCompany)e.Item.FindControl("ucCompany")).SelectedCompany)
                                                , General.GetNullableGuid(ViewState["STOREITEMID"].ToString())
                                                , General.GetNullableDecimal(ucDefaultPrice.Text)
                                                , General.GetNullableDecimal(((UserControlMaskNumber)e.Item.FindControl("ucDefaultPriceEdit")).Text)
                                                );
                gvFormDetails.Rebind();
                Rebind();
            }

        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }

    protected void gvFormDetails_NeedDataSource(object sender, GridNeedDataSourceEventArgs e)
    {
        try
        {
            ViewState["PAGENUMBER"] = ViewState["PAGENUMBER"] != null ? ViewState["PAGENUMBER"] : gvFormDetails.CurrentPageIndex + 1;
            BindData();
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }
    protected void Rebind()
    {
        gvFormDetails.SelectedIndexes.Clear();
        gvFormDetails.EditIndexes.Clear();
        gvFormDetails.DataSource = null;
        gvFormDetails.Rebind();
    }
    protected void gvFormDetails_SelectedIndexChanging(object sender, GridViewSelectEventArgs e)
    {
        //gvFormDetails.SelectedIndex = e.NewSelectedIndex;

    }


}
