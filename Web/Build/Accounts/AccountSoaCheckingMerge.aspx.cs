using System;
using System.Data;
using System.Web.UI.WebControls;
using SouthNests.Phoenix.Framework;
using SouthNests.Phoenix.Accounts;
using SouthNests.Phoenix.Common;
using System.Web.UI;
using Telerik.Web.UI;
public partial class Accounts_AccountSoaCheckingMerge : PhoenixBasePage
{
    //protected override void Render(HtmlTextWriter writer)
    //{
    //    foreach (GridViewRow r in gvOwnersAccount.Rows)
    //    {
    //        if (r.RowType == DataControlRowType.DataRow)
    //        {
    //            Page.ClientScript.RegisterForEventValidation(gvOwnersAccount.UniqueID, "Select$" + r.RowIndex.ToString());
    //        }
    //    }

    //    base.Render(writer);
    //}

    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {
            SessionUtil.PageAccessRights(this.ViewState);
            //PhoenixToolbar toolbargrid = new PhoenixToolbar();
            //toolbargrid.AddImageButton("../Accounts/AccountsSoaChecking.aspx", "Export to Excel", "icon_xls.png", "Excel");
            //toolbargrid.AddImageLink("javascript:CallPrint('gvOwnersAccount')", "Print Grid", "icon_print.png", "PRINT");
            //MenuOrderForm.AccessRights = this.ViewState;
            //MenuOrderForm.MenuList = toolbargrid.Show();

            PhoenixToolbar toolbarmain = new PhoenixToolbar();
            toolbarmain.AddButton("Merge", "MERGE",ToolBarDirection.Right);
            MenuGeneral.AccessRights = this.ViewState;
            MenuGeneral.MenuList = toolbarmain.Show();
           
            //PhoenixToolbar toolbarsub = new PhoenixToolbar();
            //toolbarsub.AddButton("Details", "DETAILS");
            //toolbarsub.AddButton("Voucher", "VOUCHER");
            ////toolbarsub.AddButton("Query", "QUERY");
            //MenuGenralSub.AccessRights = this.ViewState;
            //MenuGenralSub.MenuList = toolbarsub.Show();
            //MenuGenralSub.SelectedMenuIndex = 0;

            cmdHiddenSubmit.Attributes.Add("style", "display:none;");
            if (!IsPostBack)
            {
                ViewState["PAGENUMBER"] = 1;
                ViewState["SORTEXPRESSION"] = null;
                ViewState["SORTDIRECTION"] = null;
                ViewState["accountid"] = Request.QueryString["accountid"].ToString();
                ViewState["vouchernumber"] = Request.QueryString["vouchernumber"].ToString();
            }
            //BindData();
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }

    protected void ShowExcel()
    {
        //int iRowCount = 0;
        //int iTotalPageCount = 0;

        DataSet ds = new DataSet();

        string[] alColumns = { "FLDDEBITNOTEREFERENCE", "FLDTYPE", "FLDACCOUNTCODE", "FLDDESCRIPTION", "FLDNAME", "FLDHARDNAME", "FLDQUICKNAME", "FLDSOASTATUSNAME" };
        string[] alCaptions = { "Statement Reference", "Type", "Account Code", "Account Code Description", "Billing Party", "Year", "Month", "Status" };

        string sortexpression;
        int? sortdirection = null;
        sortexpression = (ViewState["SORTEXPRESSION"] == null) ? null : (ViewState["SORTEXPRESSION"].ToString());
        if (ViewState["SORTDIRECTION"] != null)
            sortdirection = Int32.Parse(ViewState["SORTDIRECTION"].ToString());

        ds = PhoenixAccountsSoaChecking.SoaCheckingVoucherMergeSearch(General.GetNullableInteger(ViewState["accountid"].ToString()), ViewState["vouchernumber"].ToString());

        Response.AddHeader("Content-Disposition", "attachment; filename=Requisition.xls");
        Response.ContentType = "application/vnd.msexcel";
        Response.Write("<TABLE BORDER='0' CELLPADDING='2' CELLSPACING='0' width='100%'>");
        Response.Write("<tr>");
        Response.Write("<td colspan='" + (alColumns.Length - 2).ToString() + "'><img src='http://" + Request.Url.Authority + Session["images"] + "/esmlogo4_small.png" + "' />");
        Response.Write("<h3><center>Purchase order form</center></h3></td>");
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

    protected void MenuGenralSub_TabStripCommand(object sender, EventArgs e)
    {
        RadToolBarEventArgs dce = (RadToolBarEventArgs)e;
        string CommandName = ((RadToolBarButton)dce.Item).CommandName;
    }

    protected void MenuGeneral_TabStripCommand(object sender, EventArgs e)
    {
        RadToolBarEventArgs dce = (RadToolBarEventArgs)e;
        string CommandName = ((RadToolBarButton)dce.Item).CommandName;

        if (CommandName.ToUpper().Equals("MERGE"))
        {
            string selectedrequests="";

            getSelectedRequests(ref selectedrequests);
            PhoenixAccountsSoaChecking.SoaCheckingVoucherMerge(PhoenixSecurityContext.CurrentSecurityContext.UserCode,selectedrequests);

            BindData();
            gvOwnersAccount.Rebind();
        }
    }

    protected void getSelectedRequests(ref string selectedrequests)
    {
        //string selectedrequests = "";
        for (int i = 0; i < gvOwnersAccount.Items.Count; i++)
        {
            CheckBox cb = (CheckBox)gvOwnersAccount.Items[i].FindControl("chkChecked");
            RadLabel lblproid = (RadLabel)gvOwnersAccount.Items[i].FindControl("lblVoucherId");

            if (cb != null && lblproid != null && (cb.Checked == true))
            {
                selectedrequests += lblproid.Text;
                selectedrequests += ",";
            }
        }
        if (selectedrequests.Length > 0)
        {
            selectedrequests = selectedrequests.Remove(selectedrequests.Length - 1);
        }
    }

    protected void MenuOrderForm_TabStripCommand(object sender, EventArgs e)
    {
        RadToolBarEventArgs dce = (RadToolBarEventArgs)e;
        string CommandName = ((RadToolBarButton)dce.Item).CommandName;
        try
        {

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

        string[] alColumns = { "FLDDEBITNOTEREFERENCE", "FLDTYPE", "FLDACCOUNTCODE", "FLDDESCRIPTION", "FLDNAME", "FLDHARDNAME", "FLDQUICKNAME", "FLDSOASTATUSNAME" };
        string[] alCaptions = { "Statement Reference", "Type", "Account Code", "Account Code Description", "Billing Party", "Year", "Month", "Status" };

        string sortexpression = (ViewState["SORTEXPRESSION"] == null) ? null : (ViewState["SORTEXPRESSION"].ToString());
        sortexpression = (ViewState["SORTEXPRESSION"] == null) ? null : (ViewState["SORTEXPRESSION"].ToString());

        int? sortdirection = null;

        sortexpression = (ViewState["SORTEXPRESSION"] == null) ? null : (ViewState["SORTEXPRESSION"].ToString());
        if (ViewState["SORTDIRECTION"] != null)
            sortdirection = Int32.Parse(ViewState["SORTDIRECTION"].ToString());

        if (ViewState["ROWCOUNT"] == null || Int32.Parse(ViewState["ROWCOUNT"].ToString()) == 0)
            iRowCount = 10;
        else
            iRowCount = Int32.Parse(ViewState["ROWCOUNT"].ToString());

        DataSet ds = new DataSet();

        ds = PhoenixAccountsSoaChecking.SoaCheckingVoucherMergeSearch(General.GetNullableInteger(ViewState["accountid"].ToString()), ViewState["vouchernumber"].ToString());
        gvOwnersAccount.DataSource = ds;
       

        ViewState["ROWCOUNT"] = iRowCount;
        ViewState["TOTALPAGECOUNT"] = iTotalPageCount;
    }

  

    protected void cmdSearch_Click(object sender, EventArgs e)
    {
        BindData();
    }
        

    protected void cmdHiddenSubmit_Click(object sender, EventArgs e)
    {
        BindData();
    }

    protected void gvOwnersAccount_NeedDataSource(object sender, Telerik.Web.UI.GridNeedDataSourceEventArgs e)
    {
        BindData();
    }
}
