using System;
using System.Data;
using System.Web.UI;
using System.Web.UI.WebControls;
using SouthNests.Phoenix.Framework;
using SouthNests.Phoenix.Registers;
using System.Web.UI.HtmlControls;
using Telerik.Web.UI;

public partial class RegistersVesselAccount : PhoenixBasePage
{
    protected void Page_Load(object sender, EventArgs e)
    {
        SessionUtil.PageAccessRights(this.ViewState);
        PhoenixToolbar toolbar = new PhoenixToolbar();

        toolbar.AddButton("Save", "SAVE", ToolBarDirection.Right);
        toolbar.AddButton("New", "NEW", ToolBarDirection.Right);
        MenuVesselAccount.AccessRights = this.ViewState;
        MenuVesselAccount.MenuList = toolbar.Show();
        //MenuVesselAccount.SetTrigger(pnlVesselAccount);

        PhoenixToolbar toolbargrid = new PhoenixToolbar();
        toolbargrid.AddFontAwesomeButton("../Registers/RegistersVesselAccount.aspx", "Export to Excel", "<i class=\"fas fa-file-excel\"></i>", "Excel");
        toolbargrid.AddFontAwesomeButton("javascript:CallPrint('gvVesselAccount')", "Print Grid", "<i class=\"fas fa-print\"></i>", "PRINT");
        toolbargrid.AddFontAwesomeButton("../Registers/RegistersVesselAccount.aspx", "Find", "<i class=\"fas fa-search\"></i>", "FIND");
        toolbargrid.AddFontAwesomeButton("../Registers/RegistersVesselAccount.aspx", "Clear Filter", "<i class=\"fas fa-eraser\"></i>", "CLEAR");
        MenuRegistersVesselAccount.AccessRights = this.ViewState;
        MenuRegistersVesselAccount.MenuList = toolbargrid.Show();
        txtAccountId.Attributes.Add("style", "display:none");
        //txtVesselAccountId.Attributes.Add("style", "display:none");
        if (!IsPostBack)
        {
            ViewState["PAGENUMBER"] = 1;
            ViewState["SORTEXPRESSION"] = null;
            ViewState["SORTDIRECTION"] = null;
            gdVesselAccount.PageSize = int.Parse(PhoenixGeneralSettings.CurrentGeneralSetting.Records);
        }
    }

    protected void VesselAccount_TabStripCommand(object sender, EventArgs e)
    {
        RadToolBarEventArgs dce = (RadToolBarEventArgs)e;
        string CommandName = ((RadToolBarButton)dce.Item).CommandName;

        if (CommandName.ToUpper().Equals("NEW"))
        {
            ViewState["VESSELID"] = null;
            Reset();
        }
        if (CommandName.ToUpper().Equals("SAVE"))
        {
            int? iVesselAccountId = null;
            if (!IsValidVesselAccount())
            {
                ucError.Visible = true;
                return;
            }
            try
            {
                if (txtVesselAccountId.Text != null && txtVesselAccountId.Text != string.Empty)
                    iVesselAccountId = int.Parse(txtVesselAccountId.Text);

                PhoenixRegistersVesselAccount.SaveVesselAccount(PhoenixSecurityContext.CurrentSecurityContext.UserCode, iVesselAccountId,
                                            Convert.ToInt32(ViewState["VESSELID"]), txtVesselName.Text, null,
                                            txtAccountId.Text, chkAccessAllowed.Checked == true ? 1 : 0, txtVesselCode.Text,
                                            General.GetNullableInteger(ViewState["VESSELHISTORYID"].ToString()), chkOfficeExpense.Checked == true ? 1 : 0, General.GetNullableInteger(ddlReportsCurrency.SelectedValue));
                Reset();
                Rebind();
            }
            catch (Exception ex)
            {
                ucError.ErrorMessage = ex.Message;
                ucError.Visible = true;
            }
        }
    }

    protected void Reset()
    {
        ViewState["VESSELID"] = null;
        txtVesselName.Text = "";
        txtAccountId.Text = "";
        txtAccountCode.Text = "";
        txtAccountDescription.Text = "";
        txtVesselAccountId.Text = "";
        txtVesselCode.Text = "";
        chkAccessAllowed.Checked = false;
        chkOfficeExpense.Checked = false;
        ddlReportsCurrency.SelectedValue = "0";
    }

    private void BindData()
    {
        int iRowCount = 0;
        int iTotalPageCount = 0;

        string[] alColumns = { "FLDVESSELNAME", "FLDVESSELCODE", "FLDOWNERNAME", "FLDACCOUNTCODE", "FLDACCOUNTCODEDESCRIPTION", "FLDACCESSALLOWEDDESCRIPTION", "FLDISOFFICEEXPENSEDESCRIPTION" };
        string[] alCaptions = { "Vessel Name", "Vessel Code", "Principal", "Account Code", "Description", "Allow New PO", "Office Expense" };

        string sortexpression = (ViewState["SORTEXPRESSION"] == null) ? null : (ViewState["SORTEXPRESSION"].ToString());
        int? sortdirection = null;
        if (ViewState["SORTDIRECTION"] != null)
            sortdirection = Int32.Parse(ViewState["SORTDIRECTION"].ToString());


        DataSet ds = PhoenixRegistersVesselAccount.VesselAccountSearch(
                        PhoenixSecurityContext.CurrentSecurityContext.UserCode
                        , txtVesselNameSearch.Text.Trim()
                        , sortexpression, sortdirection
                        , (int)ViewState["PAGENUMBER"], gdVesselAccount.PageSize
                        , ref iRowCount, ref iTotalPageCount);

        General.SetPrintOptions("gvVesselAccount", "Vessel Account", alCaptions, alColumns, ds);


        gdVesselAccount.DataSource = ds;
        gdVesselAccount.VirtualItemCount = iRowCount;
        ViewState["ROWCOUNT"] = iRowCount;
        ViewState["TOTALPAGECOUNT"] = iTotalPageCount;
    }
    protected void VesselAccountMenu_TabStripCommand(object sender, EventArgs e)
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
        if (CommandName.ToUpper().Equals("CLEAR"))
        {
            txtVesselNameSearch.Text = "";
            Rebind();
        }
    }

    protected void ShowExcel()
    {
        int iRowCount = 0;
        int iTotalPageCount = 0;
        string[] alColumns = { "FLDVESSELNAME", "FLDVESSELCODE", "FLDOWNERNAME", "FLDACCOUNTCODE", "FLDACCOUNTCODEDESCRIPTION", "FLDACCESSALLOWEDDESCRIPTION", "FLDISOFFICEEXPENSEDESCRIPTION" };
        string[] alCaptions = { "Vessel Name", "Vessel Code", "Principal", "Account Code", "Description", "Allow New PO", "Office Expense" };
        string sortexpression;
        int? sortdirection = null;

        sortexpression = (ViewState["SORTEXPRESSION"] == null) ? null : (ViewState["SORTEXPRESSION"].ToString());
        if (ViewState["SORTDIRECTION"] != null)
            sortdirection = Int32.Parse(ViewState["SORTDIRECTION"].ToString());

        if (ViewState["ROWCOUNT"] == null || Int32.Parse(ViewState["ROWCOUNT"].ToString()) == 0)
            iRowCount = 10;
        else
            iRowCount = Int32.Parse(ViewState["ROWCOUNT"].ToString());


        DataSet ds = PhoenixRegistersVesselAccount.VesselAccountSearch(
                        PhoenixSecurityContext.CurrentSecurityContext.UserCode
                        , txtVesselNameSearch.Text.Trim()
                        , sortexpression, sortdirection
                        , 1, iRowCount
                        , ref iRowCount
                        , ref iTotalPageCount);


        Response.AddHeader("Content-Disposition", "attachment; filename=VesselAccount.xls");
        Response.ContentType = "application/vnd.msexcel";
        Response.Write("<TABLE BORDER='0' CELLPADDING='2' CELLSPACING='0' width='100%'>");
        Response.Write("<tr>");
        Response.Write("<td><img src='http://" + Request.Url.Authority + Session["images"] + "/esmlogo4_small.png" + "' /></td>");
        Response.Write("<td><h3>Vessel Account Register</h3></td>");
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

    private bool IsValidVesselAccount()
    {
        ucError.HeaderMessage = "Please provide the following required information";

        if (txtVesselName.Text.Equals(""))
            ucError.ErrorMessage = "Vessel Name is required.";

        if (txtAccountId.Text.Equals(""))
            ucError.ErrorMessage = "Account Code is required.";

        return (!ucError.IsError);
    }
    protected void gdVesselAccount_ItemCommand(object sender, GridCommandEventArgs e)
    {
        if (e.CommandName == "Page")
        {
            ViewState["PAGENUMBER"] = null;
        }
        if (e.CommandName.ToUpper().Equals("SORT"))
            return;
        if (e.CommandName.ToUpper().Equals("DELETE"))
        {
            string[] strSelectedVessel;
            strSelectedVessel = e.CommandArgument.ToString().Split('^');
            ViewState["VESSELID"] = strSelectedVessel[0];
            ViewState["VESSELCODE"] = strSelectedVessel[1];
            string strvesselaccountid = strSelectedVessel[2];
            PhoenixRegistersVesselAccount.DeleteVesselAccount(PhoenixSecurityContext.CurrentSecurityContext.UserCode, General.GetNullableInteger(strvesselaccountid));
            Rebind();
        }

    }
    protected void Rebind()
    {
        gdVesselAccount.SelectedIndexes.Clear();
        gdVesselAccount.EditIndexes.Clear();
        gdVesselAccount.DataSource = null;
        gdVesselAccount.Rebind();
    }
    protected void gdVesselAccount_ItemDataBound(Object sender, GridItemEventArgs e)
    {
        LinkButton del = (LinkButton)e.Item.FindControl("cmdDelete");
        if (del != null) del.Visible = SessionUtil.CanAccess(this.ViewState, del.CommandName);

        if (e.Item is GridEditableItem)
        {
            {
                LinkButton db = (LinkButton)e.Item.FindControl("cmdDelete");
                if (db != null) db.Attributes.Add("onclick", "return fnConfirmDelete(event); return false;");
            }
        }

        //if (e.Item is GridEditableItem)
        //{
        //    if (ViewState["SORTEXPRESSION"] != null)
        //    {
        //        HtmlImage img = (HtmlImage)e.Item.FindControl(ViewState["SORTEXPRESSION"].ToString());
        //        if (img != null)
        //        {
        //            if (ViewState["SORTDIRECTION"] == null || ViewState["SORTDIRECTION"].ToString() == "0")
        //                img.Src = Session["images"] + "/arrowUp.png";
        //            else
        //                img.Src = Session["images"] + "/arrowDown.png";

        //            img.Visible = true;
        //        }
        //    }
        //}
    }
  
    protected void gdVesselAccount_SortCommand(object sender, GridSortCommandEventArgs e)
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
    protected void VesselNameClick(object sender, CommandEventArgs e)
    {
        string[] strSelectedVessel = new string[3];
        strSelectedVessel = e.CommandArgument.ToString().Split('^');
        ViewState["VESSELID"] = strSelectedVessel[0];
        ViewState["VESSELCODE"] = strSelectedVessel[1];
        ViewState["VESSELHISTORYID"] = strSelectedVessel[2];
        VesselAccountEdit();
    }

    protected void VesselAccountEdit()
    {
        if (ViewState["VESSELID"] != null)
        {
            DataSet dsvesselaccount = PhoenixRegistersVesselAccount.EditVesselAccount
                (PhoenixSecurityContext.CurrentSecurityContext.UserCode, Convert.ToInt32(ViewState["VESSELID"]), ViewState["VESSELCODE"].ToString(), General.GetNullableInteger(ViewState["VESSELHISTORYID"].ToString()));

            if (dsvesselaccount.Tables.Count > 0)
            {
                DataRow drvesselaccount = dsvesselaccount.Tables[0].Rows[0];
                txtVesselName.Text = drvesselaccount["FLDVESSELNAME"].ToString();
                txtVesselCode.Text = ViewState["VESSELCODE"].ToString();
                txtAccountId.Text = drvesselaccount["FLDACCOUNTCODE"].ToString();

                txtAccountId.Text = drvesselaccount["FLDACCOUNTID"].ToString();
                txtAccountCode.Text = drvesselaccount["FLDACCOUNTCODE"].ToString();
                txtAccountDescription.Text = drvesselaccount["FLDACCOUNTDESCRIPTION"].ToString();
                txtVesselAccountId.Text = drvesselaccount["FLDVESSELACCOUNTID"].ToString();

                if (drvesselaccount["FLDACCESSALLOWED"].ToString() == "1")
                {
                    chkAccessAllowed.Checked = true;
                }
                if (drvesselaccount["FLDACCESSALLOWED"].ToString() == "0" || drvesselaccount["FLDACCESSALLOWED"].ToString() == "")
                {
                    chkAccessAllowed.Checked = false;
                }
                if (drvesselaccount["FLDISOFFICEEXPENSE"].ToString() == "1")
                {
                    chkOfficeExpense.Checked = true;
                }
                if (drvesselaccount["FLDISOFFICEEXPENSE"].ToString() == "0" || drvesselaccount["FLDISOFFICEEXPENSE"].ToString() == "")
                {
                    chkOfficeExpense.Checked = false;
                }
                ddlReportsCurrency.SelectedValue = drvesselaccount["FLDREPORTSCURRENCY"].ToString();
                Rebind();
            }
        }
    }

    protected void AccountCodeClick(object sender, CommandEventArgs e)
    {
        ViewState["ACCOUNTCODEID"] = e.CommandArgument;
        VesselAccountCodeEdit();
    }

    protected void VesselAccountCodeEdit()
    {
        if (ViewState["VESSELID"] != null)
        {
            DataSet dsvesselaccountcode = PhoenixRegistersAccount.EditAccount
                  (Int16.Parse(ViewState["ACCOUNTCODEID"].ToString()));

            if (dsvesselaccountcode.Tables.Count > 0)
            {
                DataRow drvesselaccountcode = dsvesselaccountcode.Tables[0].Rows[0];
                txtAccountId.Text = drvesselaccountcode["FLDACCOUNTCODE"].ToString();
                Rebind();
            }
        }
    }

    public StateBag ReturnViewState()
    {
        return ViewState;
    }

    protected void gdVesselAccount_NeedDataSource(object sender, GridNeedDataSourceEventArgs e)
    {
        try
        {
            ViewState["PAGENUMBER"] = ViewState["PAGENUMBER"] != null ? ViewState["PAGENUMBER"] : gdVesselAccount.CurrentPageIndex + 1;
            BindData();
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }
}
