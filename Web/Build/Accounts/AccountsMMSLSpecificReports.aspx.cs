using System;
using System.Data;
using System.Web.UI.WebControls;
using SouthNests.Phoenix.Framework;
using System.Collections.Specialized;
using System.Web.UI.HtmlControls;
using SouthNests.Phoenix.Accounts;
using System.Web.UI;
using SouthNests.Phoenix.Common;
using System.Text;
using SouthNests.Phoenix.Registers;
using SouthNests.Phoenix.Export2XL;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using OfficeOpenXml;
using OfficeOpenXml.Style;
using System.Drawing;
using System.IO;
using Telerik.Web.UI;

public partial class Accounts_AccountsMMSLSpecificReports : PhoenixBasePage
{
    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {
            SessionUtil.PageAccessRights(this.ViewState);

            PhoenixToolbar toolbarmain = new PhoenixToolbar();
            //toolbarmain.AddButton("SOA", "SOA");
            //toolbarmain.AddButton("Budget Code", "VOUCHER");

            PhoenixToolbar toolbarowneraccounts = new PhoenixToolbar();
            MenuAccountsowner.AccessRights = this.ViewState;
            toolbarowneraccounts.AddImageButton("../Accounts/AccountsMMSLSpecificReports.aspx", "Find", "search.png", "FIND");
            //if (SessionUtil.CanAccess(this.ViewState, "PRINT"))
            //    toolbarowneraccounts.AddImageLink("javascript:CallPrint('gvOwnersAccount')", "Print Grid", "icon_print.png", "PRINT");
            //if (SessionUtil.CanAccess(this.ViewState, "Excel"))
            //    toolbarowneraccounts.AddImageButton("../Accounts/AccountsKOYOSpecificReports.aspx", "Export to Excel", "icon_xls.png", "Excel");
            MenuAccountsowner.AccessRights = this.ViewState;
            MenuAccountsowner.MenuList = toolbarowneraccounts.Show();

            MenuOrderFormMain.Title = "MMSL Specific Reports";
            MenuOrderFormMain.AccessRights = this.ViewState;
            MenuOrderFormMain.MenuList = toolbarmain.Show();
            if (!IsPostBack)
            {
                ViewState["PAGENUMBER"] = 1;
                ViewState["SORTEXPRESSION"] = null;
                ViewState["SORTDIRECTION"] = null;
                gvOwnersAccount.PageSize = int.Parse(PhoenixGeneralSettings.CurrentGeneralSetting.Records);
                ViewState["accountid"] = "";
                ViewState["debitnotereference"] = "";
                ViewState["Ownerid"] = "";
                //ddlVessel.Items.Insert(0, new ListItem("--Select--", "Dummy"));
                ucOwner_Onchange();
            }
            //uservesselmap();
            //BindData();
           

        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }

    }

    protected void ucOwner_Onchange()
    {
        //ddlVessel.Items.Clear();
        DataSet dsveaaelname = new DataSet();
        dsveaaelname = PhoenixCommonBudgetGroupAllocation.AccountsOwnerVessellist(General.GetNullableInteger("4221"));
        ddlVessel.DataSource = dsveaaelname;
        ddlVessel.DataTextField = "FLDVESSELNAME";
        ddlVessel.DataValueField = "FLDVESSELID";
        ddlVessel.DataBind();
        //ddlVessel.Items.Insert(0, new ListItem("--Select--", "Dummy"));
    }

    //-----------------------------------------------------------------






    //---------------------------------------------------------------

    //protected void uservesselmap()
    //{
    //    if (ddlVessel.Items.Count == 0)
    //    {
    //        ddlVessel.DataSource = PhoenixRegistersVessel.ListAllVessel(1);
    //        ddlVessel.DataTextField = "FLDVESSELNAME";
    //        ddlVessel.DataValueField = "FLDVESSELID";
    //        ddlVessel.DataBind();
    //        ddlVessel.Items.Insert(0, new ListItem("--Select--", ""));
    //    }
    //}

    protected void MenuOrderFormMain_TabStripCommand(object sender, EventArgs e)
    {
        RadToolBarEventArgs dce = (RadToolBarEventArgs)e;
        string CommandName = ((RadToolBarButton)dce.Item).CommandName;
        try
        {
            if (CommandName.ToUpper().Equals("VOUCHER"))
            {
                if (ViewState["accountid"].ToString() == "" || ViewState["debitnotereference"].ToString() == "" || ViewState["Ownerid"].ToString() == "")
                {
                    ucError.ErrorMessage = "Select a Account";
                    ucError.Visible = true;
                    return;
                }
                else
                    Response.Redirect("../Accounts/AccountsOwnersStatementOfAccounts.aspx?accountid=" + ViewState["accountid"].ToString() + "&debitnotereference=" + ViewState["debitnotereference"].ToString() + "&Ownerid=" + ViewState["Ownerid"].ToString(), true);
            }
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }

    protected void AccountsownerMenu_TabStripCommand(object sender, EventArgs e)
    {
        RadToolBarEventArgs dce = (RadToolBarEventArgs)e;
        string CommandName = ((RadToolBarButton)dce.Item).CommandName;
        if (CommandName.ToUpper().Equals("FIND"))
        {

            ViewState["PAGENUMBER"] = 1;
            BindData();
           
        }

        else if (CommandName.ToUpper().Equals("EXCEL"))
        {
            ShowExcel();
        }
    }

    protected void ShowExcel()
    {
        int iRowCount = 0;
        int iTotalPageCount = 0;

        DataSet ds = new DataSet();

        string[] alColumns = { "FLDVESSELNAME", "FLDDEBITNOTEREFERENCE", "FLDHARDNAME", "FLDQUICKNAME" };
        string[] alCaptions = { "Vessel Name", "SOA Reference", "Month", "Year" };

        string sortexpression;
        int? sortdirection = null;
        sortexpression = (ViewState["SORTEXPRESSION"] == null) ? null : (ViewState["SORTEXPRESSION"].ToString());
        if (ViewState["SORTDIRECTION"] != null)
            sortdirection = Int32.Parse(ViewState["SORTDIRECTION"].ToString());

        if (ViewState["ROWCOUNT"] == null || Int32.Parse(ViewState["ROWCOUNT"].ToString()) == 0)
            iRowCount = 10;
        else
            iRowCount = Int32.Parse(ViewState["ROWCOUNT"].ToString());

        ds = PhoenixAccountsOwnerStatementOfAccounts.StatementOfAccountsSearch(PhoenixSecurityContext.CurrentSecurityContext.UserCode
                                                , sortexpression, sortdirection,
                                                (int)ViewState["PAGENUMBER"],
                                                iRowCount,
                                                ref iRowCount,
                                                ref iTotalPageCount, General.GetNullableInteger(null), "", "", "", General.GetNullableInteger(null), General.GetNullableInteger(null));

        Response.AddHeader("Content-Disposition", "attachment; filename=StatementOfAccounts.xls");
        Response.ContentType = "application/vnd.msexcel";
        Response.Write("<TABLE BORDER='0' CELLPADDING='2' CELLSPACING='0' width='100%'>");
        Response.Write("<tr>");
        Response.Write("<td colspan='" + (alColumns.Length - 2).ToString() + "'><img src='http://" + Request.Url.Authority + Session["images"] + "/esmlogo4_small.png" + "' />");
        Response.Write("<h3><center>Statement of Accounts</center></h3></td>");
        //Response.Write("<td colspan='" + (alColumns.Length - 2).ToString() + "'>&nbsp;</td>");
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
                Response.Write(dr[alColumns[i]].GetType().Equals(typeof(DateTime)) ? General.GetDateTimeToString(dr[alColumns[i]].ToString()) : dr[alColumns[i]]);
                Response.Write("</td>");
            }
            Response.Write("</tr>");
        }
        Response.Write("</TABLE>");
        Response.End();
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
                BindData();
              
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

    private void BindData()
    {
        int iRowCount = 0;
        int iTotalPageCount = 0;

        string[] alColumns = { "FLDVESSELNAME", "FLDDEBITNOTEREFERENCE", "FLDHARDNAME", "FLDQUICKNAME" };
        string[] alCaptions = { "Vessel Name", "SOA Reference", "Month", "Year" };

        string sortexpression = (ViewState["SORTEXPRESSION"] == null) ? null : (ViewState["SORTEXPRESSION"].ToString());
        sortexpression = (ViewState["SORTEXPRESSION"] == null) ? null : (ViewState["SORTEXPRESSION"].ToString());

        int? sortdirection = null;

        sortexpression = (ViewState["SORTEXPRESSION"] == null) ? null : (ViewState["SORTEXPRESSION"].ToString());
        //if (ViewState["SORTDIRECTION"] != null)
        //    sortdirection = Int32.Parse(ViewState["SORTDIRECTION"].ToString());

        //if (ViewState["ROWCOUNT"] == null || Int32.Parse(ViewState["ROWCOUNT"].ToString()) == 0)
        //    iRowCount = 10;
        //else
        //    iRowCount = Int32.Parse(ViewState["ROWCOUNT"].ToString());

        DataSet ds = new DataSet();

        ds = PhoenixAccountsOwnerStatementOfAccounts.StatementOfAccountsSearch(PhoenixSecurityContext.CurrentSecurityContext.UserCode
                                                 , sortexpression, sortdirection,
                                                 (int)ViewState["PAGENUMBER"],
                                                gvOwnersAccount.PageSize,
                                                 ref iRowCount,
                                                 ref iTotalPageCount,
                                                 General.GetNullableInteger(ddlVessel.SelectedValue),
                                                 General.GetNullableString(null),
                                                 General.GetNullableString(null),
                                                 General.GetNullableString(null),
                                                 null,
                                                 null);

        gvOwnersAccount.DataSource = ds;
        gvOwnersAccount.VirtualItemCount = iRowCount;


    
        ViewState["ROWCOUNT"] = iRowCount;
        ViewState["TOTALPAGECOUNT"] = iTotalPageCount;
        General.SetPrintOptions("gvOwnersAccount", "Statement of Accounts", alCaptions, alColumns, ds);
    }

    //protected void cmdSort_Click(object sender, EventArgs e)
    //{
    //    ImageButton ib = (ImageButton)sender;
    //    try
    //    {
    //        gvOwnersAccount.SelectedIndex = -1;
    //        ViewState["SORTEXPRESSION"] = ib.CommandName;
    //        ViewState["SORTDIRECTION"] = int.Parse(ib.CommandArgument);
    //        BindData();
    //        SetPageNavigator();
    //    }
    //    catch (Exception ex)
    //    {
    //        ucError.ErrorMessage = ex.Message;
    //        ucError.Visible = true;
    //    }

    //}

    protected void cmdSearch_Click(object sender, EventArgs e)
    {
        BindData();
     
    }


    protected void gvOwnersAccount_ItemDataBound(object sender, GridItemEventArgs e)

    {
       
        if (e.Item is GridDataItem)
        {
            RadLabel lblAccountId = (RadLabel)e.Item.FindControl("lblAccountId");
            RadLabel lblDebitNoteReference = (RadLabel)e.Item.FindControl("lblSoaReference");
            RadLabel lblOwnerid = (RadLabel)e.Item.FindControl("lblOwnerId");
            ImageButton cmdExport2XL = (ImageButton)e.Item.FindControl("cmdExport2XL");

            LinkButton lbr = (LinkButton)e.Item.FindControl("lnkAccountCOde");

            //lbr.Attributes.Add

            DataRowView drv = (DataRowView)e.Item.DataItem;

            ImageButton att = (ImageButton)e.Item.FindControl("cmdAtt");
            if (att != null)
            {
                att.Visible = SessionUtil.CanAccess(this.ViewState, att.CommandName);
                if (drv["FLDISATTACHMENT"].ToString() == "0")
                    att.ImageUrl = Session["images"] + "/no-attachment.png";
                att.Attributes.Add("onclick", "javascript:parent.Openpopup('att','','../Common/CommonFileAttachment.aspx?dtkey=" + drv["FLDDTKEY"].ToString() + "&mod="
                    + PhoenixModule.ACCOUNTS + "&U=NO'); return false;");
            }

            if (cmdExport2XL != null)
            {
                cmdExport2XL.Visible = true;
                /*
                if (dr["FLDINSPECTIONCATEGORYID"].ToString() == PhoenixCommonRegisters.GetHardCode(1, 144, "INT"))
                {
                    if (dr["FLDSHORTCODE"].ToString().ToUpper().Contains("VIR") ||
                        dr["FLDSHORTCODE"].ToString().ToUpper().Contains("NAV") ||
                        dr["FLDSHORTCODE"].ToString().ToUpper().Contains("HSEQA") ||
                        dr["FLDSHORTCODE"].ToString().ToUpper().Contains("ISPS") ||
                        dr["FLDSHORTCODE"].ToString().ToUpper().Contains("ENV") ||
                        dr["FLDSHORTCODE"].ToString().ToUpper().Contains("MASTER") ||
                        dr["FLDSHORTCODE"].ToString().ToUpper().Contains("C/E"))
                        cmdExport2XL.Visible = true;
                }  
                 */
                //if (drv["FLDINSTALLCODE"] != null && drv["FLDINSTALLCODE"].ToString() != "" && int.Parse(drv["FLDINSTALLCODE"].ToString()) > 0)
                //    cmdExport2XL.Visible = false;

                if (!SessionUtil.CanAccess(this.ViewState, cmdExport2XL.CommandName))
                {
                    cmdExport2XL.Visible = false;
                }
            }

        }
    }

    protected void gvOwnersAccount_NeedDataSource(object sender, GridNeedDataSourceEventArgs e)
    {
        try
        {
            ViewState["PAGENUMBER"] = ViewState["PAGENUMBER"] != null ? ViewState["PAGENUMBER"] : gvOwnersAccount.CurrentPageIndex + 1;
            BindData();
        }

        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }

    protected void gvOwnersAccount_ItemCommand(object sender, GridCommandEventArgs e)
    {
        try
        {

            if (e.CommandName == "Page")
            {
                ViewState["PAGENUMBER"] = null;
            }


            if (e.CommandName.ToUpper().Equals("SELECT"))
            {
                GridDataItem item = (GridDataItem)e.Item;

                RadLabel lblAccountId = (RadLabel)e.Item.FindControl("lblAccountId");
                ViewState["accountid"] = (RadLabel)e.Item.FindControl("lblAccountId");

                RadLabel lblAccountCode = (RadLabel)e.Item.FindControl("lblAccountCode");

                RadLabel lblDebitNoteReference = (RadLabel)e.Item.FindControl("lblSoaReference");
                ViewState["debitnotereference"] = (RadLabel)e.Item.FindControl("lblSoaReference");

                RadLabel lblOwnerid = (RadLabel)e.Item.FindControl("lblOwnerId");
                ViewState["Ownerid"] = null;

                Response.Redirect("../Accounts/AccountsOwnerStatementOfAccountBudget.aspx?accountid="
                     + lblAccountId.Text + "&debitnoteref="
                     + lblDebitNoteReference.Text + "&accountcode="
                     + lblAccountCode.Text + "&SUPPORTINGSYN=NO", true);


            }
            if (e.CommandName.ToUpper().Equals("EXPORT2XL"))
            {
                RadLabel lblAccountId = (RadLabel)e.Item.FindControl("lblAccountId");
                RadLabel lblDebitNoteReference = (RadLabel)e.Item.FindControl("lblSoaReference");
                RadLabel lblOwnerid = (RadLabel)e.Item.FindControl("lblOwnerId");
                RadLabel lblAccountCode = (RadLabel)e.Item.FindControl("lblAccountCode");
                //Export2XLMonthlyVoucherDetails(General.GetNullableInteger(lblAccountId.Text), General.GetNullableString(lblAccountCode.Text),
                //                                                        General.GetNullableString(lblDebitNoteReference.Text), General.GetNullableInteger(lblOwnerid.Text));
                PhoenixAccounts2XL.Export2XLMonthlyVoucherDetails(General.GetNullableInteger(lblAccountId.Text), General.GetNullableString(lblAccountCode.Text),
                                                                                    General.GetNullableString(lblDebitNoteReference.Text), General.GetNullableInteger(lblOwnerid.Text));
            }

        }

        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }

    }

    //protected void gvOwnersAccount_RowCommand(object sender, GridViewCommandEventArgs e)
    //{
    //    if (e.CommandName.ToUpper().Equals("SORT"))
    //        return;

    //    GridView _gridView = (GridView)sender;
    //    int nCurrentRow = Int32.Parse(e.CommandArgument.ToString());

    //    if (e.CommandName.ToUpper().Equals("SELECT"))
    //    {
    //        Label lblAccountId = (Label)_gridView.Rows[nCurrentRow].FindControl("lblAccountId");
    //        ViewState["accountid"] = (Label)_gridView.Rows[nCurrentRow].FindControl("lblAccountId");

    //        Label lblAccountCode = (Label)_gridView.Rows[nCurrentRow].FindControl("lblAccountCode");

    //        Label lblDebitNoteReference = (Label)_gridView.Rows[nCurrentRow].FindControl("lblSoaReference");
    //        ViewState["debitnotereference"] = (Label)_gridView.Rows[nCurrentRow].FindControl("lblSoaReference");

    //        Label lblOwnerid = (Label)_gridView.Rows[nCurrentRow].FindControl("lblOwnerId");
    //        ViewState["Ownerid"] = null;

    //        Response.Redirect("../Accounts/AccountsOwnerStatementOfAccountBudget.aspx?accountid="
    //             + lblAccountId.Text + "&debitnoteref="
    //             + lblDebitNoteReference.Text + "&accountcode="
    //             + lblAccountCode.Text + "&SUPPORTINGSYN=NO", true);
    //    }

    //    if (e.CommandName.ToUpper().Equals("EXPORT2XL"))
    //    {
    //        Label lblAccountId = (Label)_gridView.Rows[nCurrentRow].FindControl("lblAccountId");
    //        Label lblDebitNoteReference = (Label)_gridView.Rows[nCurrentRow].FindControl("lblSoaReference");
    //        Label lblOwnerid = (Label)_gridView.Rows[nCurrentRow].FindControl("lblOwnerId");
    //        Label lblAccountCode = (Label)_gridView.Rows[nCurrentRow].FindControl("lblAccountCode");
    //        PhoenixAccounts2XL.Export2XLMonthlyMMSLVoucherDetails(General.GetNullableInteger(lblAccountId.Text), General.GetNullableString(lblAccountCode.Text),
    //                                                                            General.GetNullableString(lblDebitNoteReference.Text), General.GetNullableInteger(lblOwnerid.Text));
    //       // Export2XLMonthlyMMSLVoucherDetails(General.GetNullableInteger(lblAccountId.Text), General.GetNullableString(lblAccountCode.Text),
    //         //                                                                               General.GetNullableString(lblDebitNoteReference.Text), General.GetNullableInteger(lblOwnerid.Text));
    //    }
    //}

}
