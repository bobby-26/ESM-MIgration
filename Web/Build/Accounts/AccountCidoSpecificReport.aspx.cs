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
using SouthNests.Phoenix.Owners;
using Telerik.Web.UI;

public partial class AccountCidoSpecificReport : PhoenixBasePage
{
    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {
            SessionUtil.PageAccessRights(this.ViewState);

            PhoenixToolbar toolbarmain = new PhoenixToolbar();
            PhoenixToolbar toolbarowneraccounts = new PhoenixToolbar();
            MenuAccountsowner.AccessRights = this.ViewState;
            toolbarowneraccounts.AddImageButton("../Accounts/AccountCidoSpecificReport.aspx", "Find", "search.png", "FIND");
            MenuAccountsowner.AccessRights = this.ViewState;
            MenuAccountsowner.MenuList = toolbarowneraccounts.Show();


            if (!IsPostBack)
            {
                ViewState["PAGENUMBER"] = 1;
                ViewState["SORTEXPRESSION"] = null;
                ViewState["SORTDIRECTION"] = null;
                ViewState["accountid"] = "";
                ViewState["debitnotereference"] = "";
                ViewState["Ownerid"] = "";

                ucOwner_Onchange();
                gvOwnersAccount.PageSize = int.Parse(PhoenixGeneralSettings.CurrentGeneralSetting.Records);
            }

            BindData();


        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }

    }

    protected void ucOwner_Onchange()
    {

        DataSet dsveaaelname = new DataSet();
        dsveaaelname = PhoenixAccountsCidoSpecificReport.GetuservesselAccount(5039);
        ddlVessel.DataSource = dsveaaelname;
        ddlVessel.DataTextField = "FLDDESCRIPTION";
        ddlVessel.DataValueField = "FLDACCOUNTID";
        ddlVessel.DataBind();

    }
    protected void ddlVessel_changed(object sender, EventArgs e)
    {
        BindData();

    }


    protected void MenuOrderFormMain_TabStripCommand(object sender, EventArgs e)
    {

    }

    protected void AccountsownerMenu_TabStripCommand(object sender, EventArgs e)
    {
        try
        {
            RadToolBarEventArgs dce = (RadToolBarEventArgs)e;
            string CommandName = ((RadToolBarButton)dce.Item).CommandName;
            if (CommandName.ToUpper().Equals("FIND"))
            {

                ViewState["PAGENUMBER"] = 1;
                BindData();

            }
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;

        }


    }
    protected void MenuOrderForm_TabStripCommand(object sender, EventArgs e)
    {
        try
        {
            RadToolBarEventArgs dce = (RadToolBarEventArgs)e;
            string CommandName = ((RadToolBarButton)dce.Item).CommandName;
            if (CommandName.ToUpper().Equals("FIND"))
            {
                ViewState["PAGENUMBER"] = 1;
                BindData();

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


        DataSet ds = new DataSet();

        ds = PhoenixAccountsCidoSpecificReport.StatementOfAccountsSearch(5039
                                                 , sortexpression, sortdirection,
                                                 (int)ViewState["PAGENUMBER"],
                                                 General.ShowRecords(null),
                                                 ref iRowCount,
                                                 ref iTotalPageCount,
                                                 General.GetNullableInteger(ddlVessel.SelectedValue),
                                                 General.GetNullableString(null),
                                                 General.GetNullableString(null),
                                                 General.GetNullableString(null),
                                                 null,
                                                 null
                                                 );

        General.SetPrintOptions("gvOwnersAccount", "Statement of Accounts", alCaptions, alColumns, ds);
        gvOwnersAccount.DataSource = ds;
        gvOwnersAccount.VirtualItemCount = iRowCount;
        ViewState["ROWCOUNT"] = iRowCount;
        ViewState["TOTALPAGECOUNT"] = iTotalPageCount;
    }

    protected void cmdSort_Click(object sender, EventArgs e)
    {
        ImageButton ib = (ImageButton)sender;
        try
        {

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

    protected void cmdSearch_Click(object sender, EventArgs e)
    {
        BindData();

    }

    protected void Rebind()
    {
        gvOwnersAccount.SelectedIndexes.Clear();
        gvOwnersAccount.EditIndexes.Clear();
        gvOwnersAccount.DataSource = null;
        gvOwnersAccount.Rebind();
    }

    public void populatecidoReport(Guid? debitnotereferenceid)
    {
        try
        {
            //string file = HttpContext.Current.Server.MapPath("..\\Attachments\\Accounts") + @"\CidoReport.xlsm";
            //string strDestinationfile = HttpContext.Current.Server.MapPath("..\\Attachments\\Accounts\\") + @"CidoReport_Export.xlsm";

            string file = PhoenixGeneralSettings.CurrentGeneralSetting.AttachmentPath + "/Accounts/" + @"\CidoReport.xlsm";
            string strDestinationfile = PhoenixGeneralSettings.CurrentGeneralSetting.AttachmentPath + "/Accounts/" + @"CidoReport_Export.xlsm";

            //  DataSet ds;
            FileInfo Inputfile = new FileInfo(file);
            if (File.Exists(strDestinationfile)) File.Delete(strDestinationfile);
            Inputfile.CopyTo(strDestinationfile);

            FileInfo newFile = new FileInfo(strDestinationfile);

            using (ExcelPackage pck = new ExcelPackage(newFile))
            {
                PopulatecidoReport(pck, debitnotereferenceid);

                HttpContext.Current.Response.Clear();
                pck.SaveAs(HttpContext.Current.Response.OutputStream);

                HttpCookie token = HttpContext.Current.Request.Cookies["fileDownloadToken"];
                if (token != null)
                {
                    token.Expires = DateTime.Now.AddDays(-1d);
                    HttpContext.Current.Response.Cookies.Add(token);
                }

                HttpContext.Current.Response.ContentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";
                HttpContext.Current.Response.AddHeader("content-disposition", "attachment;  filename=CidoReport_Export.xlsm");


                HttpContext.Current.Response.End();
            }
            // ds = PhoenixAccountsCidoSpecificReport.CodispecificReport(debitnotereferenceid);
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;

        }

    }
    private static string GetExcelColumnName(int columnNumber)
    {
        int dividend = columnNumber;
        string columnName = String.Empty;
        int modulo;

        while (dividend > 0)
        {
            modulo = (dividend - 1) % 26;
            columnName = Convert.ToChar(65 + modulo).ToString() + columnName;
            dividend = (int)((dividend - modulo) / 26);
        }

        return columnName;
    }
    private static void PopulatecidoReport(ExcelPackage pck, Guid? debitnotereferenceid)
    {
        ExcelWorksheet ws = pck.Workbook.Worksheets["Cidodetails"];

        DataSet ds = PhoenixAccountsCidoSpecificReport.CodispecificReport(debitnotereferenceid);

        DataTable dt = ds.Tables[0];

        int nrow = 2;
        if (dt.Rows.Count > 0)
        {
            foreach (DataRow dr in dt.Rows)
            {

                ws.Cells[nrow, 1].Value = dr["FLDDEBINOTEREFERENCEID"];
                ws.Cells[nrow, 2].Value = dr["FLDTOPPARENTOWNERBUDGETGROUP"];
                ws.Cells[nrow, 3].Value = dr["FLDGRANTPARENTOWNERBUDGETGROUP"];
                ws.Cells[nrow, 4].Value = dr["FLDPARENTOWNERBUDGETGROUP"];
                //ws.Cells[nrow, 2].Value = dr["FLDOWNERBUDGETGROUPID"];
                ws.Cells[nrow, 5].Value = dr["FLDOWNERBUDGETGROUP"];
                // ws.Cells[nrow, 4].Value = dr["FLDPARENTOWNERBUDGETGROUPID"];

                // ws.Cells[nrow, 6].Value = dr["FLDGRANTPARENTOWNERBUDGETGROUPID"];

                //  ws.Cells[nrow, 8].Value = dr["FLDTOPPARENTOWNERBUDGETGROUPID"];

                ws.Cells[nrow, 6].Value = dr["FLDBUDGETCODETOTAL"];
                ws.Cells[nrow, 7].Value = dr["FLDYEAR"];
                ws.Cells[nrow, 8].Value = dr["FLDMONTH"];
                ws.Cells[nrow, 9].Value = dr["FLDBUDGETALLOCATIONTOTAL"];
                ws.Cells[nrow, 10].Value = dr["FLDTOTALMONTH"];
                ws.Cells[nrow, 11].Value = dr["FLDAVGTOTALAMOUNT"];
                ws.Cells[nrow, 12].Value = dr["FLDAVGALOCATIONBUDGETAMOUNT"];
                ws.Cells[nrow, 13].Value = dr["FLDDISBURYEARLY"];
                ws.Cells[nrow, 14].Value = dr["FLDESTYEAR"];
                ws.Cells[nrow, 15].Value = dr["FLDISBUDGETED"];
                nrow = nrow + 1;


            }
        }

        //ws.Cells[nrow, 1].Style.Fill.BackgroundColor.SetColor(Color.CornflowerBlue);
        //ws.Cells[nrow, 1].Value = "SUB-TOTAL";
        //for (int i = 2; i <= 20; i++)
        //    ws.Cells[nrow, i].Formula = "=Sum(" + GetExcelColumnName(i) + (4).ToString() + ":" + GetExcelColumnName(i) + (nrow - 1).ToString() + ")";

        //nrow = nrow + 1;
        //ws.Cells[nrow, 1].Style.Fill.BackgroundColor.SetColor(Color.CornflowerBlue);
        //ws.Cells[nrow, 1].Value = "MONTHLY (%)";
        //for (int i = 7; i <= 20; i++)
        //    ws.Cells[nrow, i].Formula = "=" + GetExcelColumnName(i) + (4).ToString() + "/$" + GetExcelColumnName(5) + (nrow - 1).ToString();
    }


    protected void gvOwnersAccount_ItemCommand(object sender, GridCommandEventArgs e)
    {
        try
        {
            if (e.CommandName.ToUpper().Equals("SORT"))
                return;


            else if (e.CommandName.ToUpper().Equals("SELECT"))
            {
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

            else if (e.CommandName.ToUpper().Equals("EXPORT2XL"))
            {
                RadLabel lblAccountId = (RadLabel)e.Item.FindControl("lblAccountId");
                RadLabel lblDebitNoteReference = (RadLabel)e.Item.FindControl("lblSoaReference");
                RadLabel lblDebitNoteReferenceid = (RadLabel)e.Item.FindControl("lblSoaReferenceid");
                RadLabel lblOwnerid = (RadLabel)e.Item.FindControl("lblOwnerId");
                RadLabel lblAccountCode = (RadLabel)e.Item.FindControl("lblAccountCode");
                populatecidoReport(General.GetNullableGuid(lblDebitNoteReferenceid.Text));

                // Rebind(); 

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

    protected void gvOwnersAccount_ItemDataBound(object sender, GridItemEventArgs e)
    {
        //  if (e.Item is GridHeaderItem)
        //   {

        // }

        if (e.Item is GridDataItem)
        {
            RadLabel lblAccountId = (RadLabel)e.Item.FindControl("lblAccountId");
            RadLabel lblDebitNoteReference = (RadLabel)e.Item.FindControl("lblSoaReference");
            RadLabel lblOwnerid = (RadLabel)e.Item.FindControl("lblOwnerId");
            ImageButton cmdExport2XL = (ImageButton)e.Item.FindControl("cmdExport2XL");
            if (cmdExport2XL != null)
            {
                cmdExport2XL.Visible = true;


                if (!SessionUtil.CanAccess(this.ViewState, cmdExport2XL.CommandName))
                {
                    cmdExport2XL.Visible = false;
                }
            }


            //   LinkButton lbr = (LinkButton)e.Item.FindControl("lnkAccountCOde");

            //lbr.Attributes.Add


            //   DataRowView drv = (DataRowView)e.Item.DataItem;

            //ImageButton att = (ImageButton)e.Item.FindControl("cmdAtt");
            //if (att != null)
            //{
            //    att.Visible = SessionUtil.CanAccess(this.ViewState, att.CommandName);
            //    if (drv["FLDISATTACHMENT"].ToString() == "0")
            //        att.ImageUrl = Session["images"] + "/no-attachment.png";
            //    att.Attributes.Add("onclick", "javascript:parent.Openpopup('att','','../Common/CommonFileAttachment.aspx?dtkey=" + drv["FLDDTKEY"].ToString() + "&mod="
            //        + PhoenixModule.ACCOUNTS + "&U=NO'); return false;");
            //}


        }
    }

    protected void gvOwnersAccount_NeedDataSource(object sender, GridNeedDataSourceEventArgs e)
    {
        ViewState["PAGENUMBER"] = ViewState["PAGENUMBER"] != null ? ViewState["PAGENUMBER"] : gvOwnersAccount.CurrentPageIndex + 1;
        BindData();
    }
}
