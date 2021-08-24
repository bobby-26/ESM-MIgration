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
using Telerik.Web.UI;

public partial class Accounts_AccountsOwnersStatementOfAccounts : PhoenixBasePage
{
    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {
            SessionUtil.PageAccessRights(this.ViewState);

            PhoenixToolbar toolbarmain = new PhoenixToolbar();
            toolbarmain.AddButton("Budget Code", "VOUCHER",ToolBarDirection.Right);
            toolbarmain.AddButton("SOA", "SOA", ToolBarDirection.Right);
            
           
            PhoenixToolbar toolbarowneraccounts = new PhoenixToolbar();
            MenuAccountsowner.AccessRights = this.ViewState;
            toolbarowneraccounts.AddImageButton("../Accounts/AccountsOwnersStatementOfAccounts.aspx", "Find", "search.png", "FIND");
            if (SessionUtil.CanAccess(this.ViewState, "PRINT"))
            toolbarowneraccounts.AddImageLink("javascript:CallPrint('gvOwnersAccount')", "Print Grid", "icon_print.png", "PRINT");
            if (SessionUtil.CanAccess(this.ViewState, "Excel"))
            toolbarowneraccounts.AddImageButton("../Accounts/AccountsOwnersStatementOfAccounts.aspx", "Export to Excel", "icon_xls.png", "Excel");
            MenuAccountsowner.AccessRights = this.ViewState;
            MenuAccountsowner.MenuList = toolbarowneraccounts.Show();

            MenuOrderFormMain.AccessRights = this.ViewState;
            MenuOrderFormMain.MenuList = toolbarmain.Show();
            MenuOrderFormMain.SelectedMenuIndex = 1;

            if (!IsPostBack)
            {
                ViewState["PAGENUMBER"] = 1;
                ViewState["SORTEXPRESSION"] = null;
                ViewState["SORTDIRECTION"] = null;
                ViewState["accountid"] = "";
                ViewState["debitnotereference"] = "";
                ViewState["Ownerid"] = "";
                uservesselmap();
                //SetDefaultYear();       
                BindLatestYearMonth();

                gvOwnersAccount.PageSize = int.Parse(PhoenixGeneralSettings.CurrentGeneralSetting.Records);
            }


            //BindData();
            //SetPageNavigator();
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }

    }

    protected void BindLatestYearMonth()
    {
        DataSet ds = PhoenixAccountsOwnerStatementOfAccount.BindLatestYearMonth();
        if (ds.Tables[0].Rows.Count > 0)
        {
            NameValueCollection nvc = Filter.CurrentSOAReference;
            if (nvc != null)
            {
                ddlVesselAccount.SelectedValue = nvc.Get("ddlVesselAccount").ToString();
                //ucYear.SelectedQuick = nvc.Get("ucYear").ToString();
                ucYear.SelectedQuick = nvc.Get("ucYear").ToString() == "Dummy" ? "0" : nvc.Get("ucYear").ToString();
                //string month = nvc.Get("ucMonth").ToString();                
                ucMonth.SelectedHard = nvc.Get("ucMonth").ToString() == "Dummy" ? "0" : nvc.Get("ucMonth").ToString();
            }
            else
            {
                DataRow dr = ds.Tables[0].Rows[0];
                ViewState["CURRENTYEARCODE"] = dr["FLDYEAR"].ToString();
                ViewState["CURRENTMONTHCODE"] = dr["FLDMONTH"].ToString();
                ucYear.SelectedQuick = dr["FLDYEAR"].ToString();
                ucMonth.SelectedHard = dr["FLDMONTH"].ToString();
            }
        }
    }

    

    protected void uservesselmap()
    {

        ddlVesselAccount.DataSource = PhoenixAccountsOwnerStatementOfAccount.GetvesselAccountid(PhoenixSecurityContext.CurrentSecurityContext.UserCode);
        ddlVesselAccount.DataTextField = "FLDDESCRIPTION";
        ddlVesselAccount.DataValueField = "FLDACCOUNTID";
        ddlVesselAccount.DataBind();
        ddlVesselAccount.Items.Insert(0, new RadComboBoxItem("--Office--", ""));
    }
   
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
             NameValueCollection criteria = new NameValueCollection();
             criteria.Clear();             
             criteria.Add("ddlVesselAccount", ddlVesselAccount.SelectedValue);
             criteria.Add("ucYear", ucYear.SelectedQuick);
             criteria.Add("ucMonth", ucMonth.SelectedHard);
             Filter.CurrentSOAReference = criteria;

             ViewState["PAGENUMBER"] = 1;
             BindData();
             gvOwnersAccount.Rebind();
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

        NameValueCollection nvc = Filter.CurrentSOAReference;

        string[] alColumns = { "FLDDESCRIPTION", "FLDDEBITNOTEREFERENCE", "FLDHARDNAME", "FLDQUICKNAME" };
        string[] alCaptions = { "Account Name", "SOA Reference", "Month", "Year" };

        string sortexpression;
        int? sortdirection = null;
        sortexpression = (ViewState["SORTEXPRESSION"] == null) ? null : (ViewState["SORTEXPRESSION"].ToString());
        if (ViewState["SORTDIRECTION"] != null)
            sortdirection = Int32.Parse(ViewState["SORTDIRECTION"].ToString());

        if (ViewState["ROWCOUNT"] == null || Int32.Parse(ViewState["ROWCOUNT"].ToString()) == 0)
            iRowCount = 10;
        else
            iRowCount = Int32.Parse(ViewState["ROWCOUNT"].ToString());

        int? month;
        int? year;
        if (ucYear.SelectedQuick.ToUpper() != "" && ucYear.SelectedQuick.ToUpper() != "DUMMY")
            year = General.GetNullableInteger(ucYear.SelectedValue);
        else
        {
            if (ViewState["CURRENTYEARCODE"] != null)
                year = General.GetNullableInteger(ViewState["CURRENTYEARCODE"].ToString());
            else
                year = General.GetNullableInteger(ucYear.SelectedValue.ToString());
        }

        if (ucMonth.SelectedHard.ToUpper() != "" && ucMonth.SelectedHard.ToUpper() != "DUMMY")
            month = General.GetNullableInteger(ucMonth.SelectedHard);
        else
        {
            if (ViewState["CURRENTMONTHCODE"] != null)
                month = General.GetNullableInteger(ViewState["CURRENTMONTHCODE"].ToString());
            else
                month = General.GetNullableInteger(ucMonth.SelectedHard);
        }

        ds = PhoenixAccountsOwnerStatementOfAccount.StatementOfAccountsSearchShowall(PhoenixSecurityContext.CurrentSecurityContext.UserCode
                                                , sortexpression, sortdirection,
                                                (int)ViewState["PAGENUMBER"],
                                                iRowCount,
                                                ref iRowCount,
                                                ref iTotalPageCount
                                                , General.GetNullableInteger(null)
                                                , null
                                                , null
                                                , null
                                                , nvc != null ? General.GetNullableInteger(nvc.Get("ucMonth")) : month
                                                , nvc != null ? General.GetNullableInteger(nvc.Get("ucYear")) : year);

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
                gvOwnersAccount.Rebind();
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

        string[] alColumns = { "FLDDESCRIPTION", "FLDDEBITNOTEREFERENCE", "FLDHARDNAME", "FLDQUICKNAME" };
        string[] alCaptions = { "Account Name", "SOA Reference", "Month", "Year" };

        string sortexpression = (ViewState["SORTEXPRESSION"] == null) ? null : (ViewState["SORTEXPRESSION"].ToString());
        sortexpression = (ViewState["SORTEXPRESSION"] == null) ? null : (ViewState["SORTEXPRESSION"].ToString());

        int? sortdirection = null;

        sortexpression = (ViewState["SORTEXPRESSION"] == null) ? null : (ViewState["SORTEXPRESSION"].ToString());
       
        int? month;
        int? year;
        if (ucYear.SelectedQuick.ToUpper() != "" && ucYear.SelectedQuick.ToUpper() != "DUMMY")
            year = General.GetNullableInteger(ucYear.SelectedValue);
        else
        {
            if (ViewState["CURRENTYEARCODE"] != null)
                year = General.GetNullableInteger(ViewState["CURRENTYEARCODE"].ToString());
            else
                year = General.GetNullableInteger(ucYear.SelectedValue.ToString());
        }

        if (ucMonth.SelectedHard.ToUpper() != "" && ucMonth.SelectedHard.ToUpper() != "DUMMY")
            month = General.GetNullableInteger(ucMonth.SelectedHard);
        else
        {
            if (ViewState["CURRENTMONTHCODE"] != null)
                month = General.GetNullableInteger(ViewState["CURRENTMONTHCODE"].ToString());
            else
                month = General.GetNullableInteger(ucMonth.SelectedHard);
        }

        DataSet ds = new DataSet();

        NameValueCollection nvc = Filter.CurrentSOAReference;

        ds = PhoenixAccountsOwnerStatementOfAccount.StatementOfAccountsSearchShowall(PhoenixSecurityContext.CurrentSecurityContext.UserCode
                                                 , sortexpression, sortdirection,
                                                 (int)ViewState["PAGENUMBER"],
                                                gvOwnersAccount.PageSize,
                                                 ref iRowCount,
                                                 ref iTotalPageCount,
                                                 nvc != null ? General.GetNullableInteger(nvc.Get("ddlVesselAccount")) : General.GetNullableInteger(ddlVesselAccount.SelectedValue),
                                                 null,
                                                 null,
                                                 null,
                                                 nvc != null ? General.GetNullableInteger(nvc.Get("ucMonth")) : month,
                                                 nvc != null ? General.GetNullableInteger(nvc.Get("ucYear")) : year );

        gvOwnersAccount.DataSource = ds;

        if (ds.Tables[0].Rows.Count > 0)
        {                     
            //ViewState["DEPOSITID"] = ds.Tables[0].Rows[0]["FLDDEPOSITID"].ToString();
            ViewState["CURRENTYEARCODE"] = ds.Tables[0].Rows[0]["FLDYEAR"].ToString();
            ViewState["CURRENTMONTHCODE"] = ds.Tables[0].Rows[0]["FLDMONTH"].ToString();

        }
        gvOwnersAccount.VirtualItemCount = iRowCount;

         ViewState["ROWCOUNT"] = iRowCount;
        ViewState["TOTALPAGECOUNT"] = iTotalPageCount;
        General.SetPrintOptions("gvOwnersAccount", "Statement of Accounts", alCaptions, alColumns, ds);
        ViewState["CURRENTYEARCODE"] = null;
        ViewState["CURRENTMONTHCODE"] = null;
    }

    

    protected void cmdSearch_Click(object sender, EventArgs e)
    {
        BindData();
        gvOwnersAccount.Rebind();
    }


    private string AddExcelStyling(string SheetName)
    {
        StringBuilder sb = new StringBuilder();
        sb.Append("<html xmlns:o='urn:schemas-microsoft-com:office:office'\n" +
        "xmlns:x='urn:schemas-microsoft-com:office:excel'\n" +
        "xmlns='http://www.w3.org/TR/REC-html40'>\n" +
        "<head>\n");

        sb.Append("<style>\n");
        sb.Append("@page");

        sb.Append("{margin:.5in .75in .5in .75in;\n");
        sb.Append("mso-header-margin:.5in;\n");

        sb.Append("mso-footer-margin:.5in;\n");
        sb.Append("mso-page-orientation:landscape;}\n");

        sb.Append("</style>\n");
        sb.Append("<!--[if gte mso 9]><xml>\n");

        sb.Append("<x:ExcelWorkbook>\n");
        sb.Append("<x:ExcelWorksheets>\n");

        sb.Append("<x:ExcelWorksheet>\n");
        sb.Append("<x:Name>" + SheetName + "</x:Name>\n");

        sb.Append("<x:WorksheetOptions>\n");
        sb.Append("<x:Print>\n");

        sb.Append("<x:ValidPrinterInfo/>\n");
        sb.Append("<x:PaperSizeIndex>9</x:PaperSizeIndex>\n");

        sb.Append("<x:HorizontalResolution>600</x:HorizontalResolution\n");
        sb.Append("<x:VerticalResolution>600</x:VerticalResolution\n");

        sb.Append("</x:Print>\n");
        sb.Append("<x:Selected/>\n");

        sb.Append("<x:DoNotDisplayGridlines/>\n");
        sb.Append("<x:ProtectContents>False</x:ProtectContents>\n");

        sb.Append("<x:ProtectObjects>False</x:ProtectObjects>\n");
        sb.Append("<x:ProtectScenarios>False</x:ProtectScenarios>\n");

        sb.Append("</x:WorksheetOptions>\n");
        sb.Append("</x:ExcelWorksheet>\n");

        sb.Append("</x:ExcelWorksheets>\n");
        sb.Append("<x:WindowHeight>12780</x:WindowHeight>\n");

        sb.Append("<x:WindowWidth>19035</x:WindowWidth>\n");
        sb.Append("<x:WindowTopX>0</x:WindowTopX>\n");

        sb.Append("<x:WindowTopY>15</x:WindowTopY>\n");
        sb.Append("<x:ProtectStructure>False</x:ProtectStructure>\n");

        sb.Append("<x:ProtectWindows>False</x:ProtectWindows>\n");
        sb.Append("</x:ExcelWorkbook>\n");

        sb.Append("</xml><![endif]-->\n");
        sb.Append("</head>\n");

        sb.Append("<body>\n");
        return sb.ToString();

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
        if (e.CommandName.ToUpper().Equals("SORT"))
            return;

      
        if (e.CommandName.ToUpper().Equals("SELECT"))
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

        if (e.CommandName.ToUpper().Equals("EXCEL"))
        {
            RadLabel lblAccountId = (RadLabel)e.Item.FindControl("lblAccountId");
            RadLabel lblDebitNoteReference = (RadLabel)e.Item.FindControl("lblSoaReference");
            RadLabel lblOwnerid = (RadLabel)e.Item.FindControl("lblOwnerId");

            DataSet ds = new DataSet();

            ds = PhoenixAccountsOwnerStatementOfAccount.StatementOfAccountsVoucher(lblDebitNoteReference.Text, General.GetNullableInteger(lblOwnerid.Text), General.GetNullableInteger(lblAccountId.Text));

            if (ds.Tables[0].Rows.Count > 0)
            {

                string[] alColumns = { "FLDOWNERBUDGETCODE", "FLDDESCRIPTION", "FLDVOUCHERDATE", "FLDVOUCHERROW", "FLDLONGDESCRIPTION", "FLDAMOUNT" };
                string[] alCaptions = { "Owner Budget Code", "Owner Budget Code Description", "Voucher Date", "Voucher Row", "Particulars", "Amount(" + ds.Tables[0].Rows[0]["FLDCURRENCYCODE"].ToString() + ")" };

                string StrSOAReference = ds.Tables[0].Rows[0]["FLDDEBITNOTEREFERENCE"].ToString();
                //string prevBcode = null;
                //string prevBdescription = null;
                //string curBcode = null;
                //string curBdescription = null;
                int columnlength = alColumns.Length;
                if (StrSOAReference.Length > 0)
                    StrSOAReference = StrSOAReference.Replace(" ", "_");

                Response.AddHeader("Content-Disposition", "attachment; filename=" + StrSOAReference + ".xls");
                Response.ContentType = "application/vnd.msexcel";

                Response.Write("<meta http-equiv=Content-Type content=\"text/html; charset=utf-8\">\n");

                Response.Write(AddExcelStyling(StrSOAReference));

                Response.Write("<TABLE BORDER='0' CELLPADDING='2' CELLSPACING='0' width='100%'>");
                Response.Write("<tr>");
                Response.Write("<td colspan='" + (alColumns.Length - 2).ToString() + "'><img src='http://" + Request.Url.Authority + Session["images"] + "/esmlogo4_small.png" + "' />");
                Response.Write("<h3><center>" + ds.Tables[0].Rows[0]["FLDACCOUNTCODEDESC"].ToString() + "<br/>" + ds.Tables[0].Rows[0]["FLDDEBITNOTEREFERENCE"].ToString() + "</center></h3></td>");

                Response.Write("</tr>");
                Response.Write("</TABLE>");
                Response.Write("<br />");
                Response.Write("<TABLE BORDER='1' CELLPADDING='1' CELLSPACING='1' width='100%'>");
                Response.Write("<tr>");
                for (int i = 0; i < alCaptions.Length; i++)
                {

                    if (i == 4)
                        Response.Write("<td colspan='2'>");
                    else
                        Response.Write("<td width='15%'>");
                    Response.Write("<b>" + alCaptions[i].ToString().Trim() + "</b>");
                    Response.Write("</td>");
                }
                Response.Write("</tr>");


                DataTable dt = ds.Tables[0];
                for (int dr = 0; dr < dt.Rows.Count; dr++)
                {
                    DataRow previousdatarow;
                    DataRow datarow;
                    Response.Write("<tr>");
                    for (int i = 0; i < alColumns.Length; i++)
                    {
                        if (i == 4)
                            Response.Write("<td colspan='2'>");
                        else
                            Response.Write("<td width='15%'>");
                        decimal s;

                        if (dr > 0)
                        {

                            previousdatarow = dt.Rows[dr - 1];
                            datarow = dt.Rows[dr];                          
                            Response.Write("<font color='black'>");
                           
                        }
                        else
                        {
                            datarow = dt.Rows[dr];
                            Response.Write("<font color='black'>");
                        }


                        Response.Write(decimal.TryParse(datarow[alColumns[i]].ToString(), out s) ? String.Format("{0:#,###,###.##}", datarow[alColumns[i]]) : datarow[alColumns[i]]);
                        Response.Write("</font>");
                        Response.Write("</td>");

                    }
                    Response.Write("</tr>");
                }
                Response.Write("</TABLE>");
                Response.Write("</body>");
                Response.Write("</html>");
                Response.End();

            }
        }
        if (e.CommandName == "Page")
        {
            ViewState["PAGENUMBER"] = null;
        }

    }

    protected void gvOwnersAccount_ItemDataBound(object sender, GridItemEventArgs e)
    {
        if (e.Item is GridDataItem)
        {
            RadLabel lblAccountId = (RadLabel)e.Item.FindControl("lblAccountId");
            RadLabel lblDebitNoteReference = (RadLabel)e.Item.FindControl("lblSoaReference");
            RadLabel lblOwnerid = (RadLabel)e.Item.FindControl("lblOwnerId");
            RadLabel lblDebitNoteReferenceid = (RadLabel)e.Item.FindControl("lblSoaReferenceid");
           

            DataRowView drv = (DataRowView)e.Item.DataItem;

            ImageButton att = (ImageButton)e.Item.FindControl("cmdAtt");
            if (att != null)
            {
                att.Visible = SessionUtil.CanAccess(this.ViewState, att.CommandName);
                if (drv["FLDISATTACHMENT"].ToString() == "0")
                    att.ImageUrl = Session["images"] + "/no-attachment.png";
                att.Attributes.Add("onclick", "javascript:parent.openNewWindow('att','','"+Session["sitepath"]+"/Common/CommonFileAttachment.aspx?dtkey=" + drv["FLDDTKEY"].ToString() + "&mod="
                    + PhoenixModule.ACCOUNTS + "&U=NO'); return false;");
            }
          
        }
    }
}
