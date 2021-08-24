using System;
using System.Data;
using System.Web.UI;
using System.Web.UI.WebControls;
using SouthNests.Phoenix.Accounts;
using SouthNests.Phoenix.Framework;
using System.Text;
using Telerik.Web.UI;
public partial class Accounts_AccountsOwnerFundRequestHistory : PhoenixBasePage
{
    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {
            SessionUtil.PageAccessRights(this.ViewState);
            PhoenixToolbar toolbargrid = new PhoenixToolbar();
            toolbargrid.AddImageButton("../Accounts/AccountsOwnerFundRequestHistory.aspx", "Export to Excel", "icon_xls.png", "Excel");
           
            MenuDebitCreditNoteGrid.AccessRights = this.ViewState;
            MenuDebitCreditNoteGrid.MenuList = toolbargrid.Show();
            if (!IsPostBack)
            {
                ViewState["PAGENUMBER"] = 1;
                ViewState["CURRENTINDEX"] = 1;
                ViewState["Source"] = "";
                if (Request.QueryString["DebitCreditNoteIdItem"] != null && Request.QueryString["DebitCreditNoteIdItem"] != string.Empty)
                {
                    ViewState["DebitCreditNoteIdItem"] = Request.QueryString["DebitCreditNoteIdItem"];

                    ViewState["ispopup"] = Request.QueryString["Ispopup"];
                }
                if (Request.QueryString["Source"] != null && Request.QueryString["Source"] != string.Empty)
                    ViewState["Source"] = Request.QueryString["Source"].ToString();
            }

           
            
            //BindData();
           // SetPageNavigator();
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
        DataSet ds = new DataSet();
        string sortexpression = (ViewState["SORTEXPRESSION"] == null) ? null : (ViewState["SORTEXPRESSION"].ToString());
        int? sortdirection = null;
        if (ViewState["SORTDIRECTION"] != null)
            sortdirection = Int32.Parse(ViewState["SORTDIRECTION"].ToString());

        ds = PhoenixAccountsOwnerDebitCreditNoteGenerate.HistoryDebitCreditNoteReceived(General.GetNullableGuid(Convert.ToString(ViewState["DebitCreditNoteIdItem"]))
                                                                                , PhoenixSecurityContext.CurrentSecurityContext.UserCode);

        //BindData();
        //SetPageNavigator();


        gvfundhistory.DataSource = ds;
        gvfundhistory.VirtualItemCount = iRowCount;
      
        ViewState["ROWCOUNT"] = iRowCount;
        ViewState["TOTALPAGECOUNT"] = iTotalPageCount;
    }

    protected void MenuDebitCreditNoteGrid_TabStripCommand(object sender, EventArgs e)
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
    protected void ShowExcel()
    {
        string refno = "";

        string[] alColumns = { "FLDAUDITDATETIME", "FLDUSERNAME", "FLDAUDITPROCEDURE", "FLDVESSELNAME", "FLDDATE", "FLDREFERENCENUMBER", "FLDSHORTCODE", "FLDTYPENAME", "FLDBANKDESCRIPTION", "FLDLINEITEM", "FLDAMOUNTINUSD", "FLDCURRENCYCODE", "FLDRECEIVEDAMOUNT", "FLDDIFFERENCE", "FLDRECEIVEDDATE" , "FLDRECEIVEDSTATUSNAME", "FLDREMARKS", "FLDOPENCLOSE", "FLDVOUCHERNUMBER" };
        string[] alCaptions = { "Audit Date", "User Name", "Procedure Name", "Vessel Name", "Date", "Reference No", "Billing Company", "Type", "Bank Receiving Funds", "Line Item", "Amount", "Currency", "Received Amount", "Difference", "Received Date", "Received Status" , "Remarks", "Open/Close", "Voucher No" };

    

        DataSet ds = PhoenixAccountsOwnerDebitCreditNoteGenerate.HistoryDebitCreditNoteReceived(General.GetNullableGuid(Convert.ToString(ViewState["DebitCreditNoteIdItem"]))
                                                                                 , PhoenixSecurityContext.CurrentSecurityContext.UserCode);
        if(ds.Tables.Count>0)
        {
            if (ds.Tables[0].Rows.Count > 0)
            {
                DataRow dr = ds.Tables[0].Rows[0];
                refno = Convert.ToString(dr["FLDREFERENCENUMBER"]);
            }  
        }

        Response.AddHeader("Content-Disposition", "attachment; filename=DebitCreditNote.xls");
        Response.ContentType = "application/vnd.msexcel";
        Response.Write("<TABLE BORDER='0' CELLPADDING='2' CELLSPACING='0' width='100%'>");
        Response.Write("<tr>");
        Response.Write("<td><img src='http://" + Request.Url.Authority + Session["images"] + "/esmlogo4_small.png" + "' /></td>");
        Response.Write("<td><h3>Fund Request History of </h3></td>");
        Response.Write("<td><h3> " + refno + "</h3></td>");
        Response.Write("<td colspan='" + (alColumns.Length +2).ToString() + "'>&nbsp;</td>");
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
                Response.Write(dr[alColumns[i]].GetType().Equals(typeof(DateTime)) ? General.GetDateTimeToString(dr[alColumns[i]].ToString()) : dr[alColumns[i]].ToString());
                Response.Write("</td>");
            }
            Response.Write("</tr>");
        }
        Response.Write("</TABLE>");
        Response.End();
    }
     

    protected void cmdSearch_Click(object sender, EventArgs e)
    {
        
        BindData();
       // SetPageNavigator();
    }

    //protected void cmdGo_Click(object sender, EventArgs e)
    //{
    //    gvfundhistory.EditIndex = -1;
    //    gvfundhistory.SelectedIndex = -1;
    //    int result;
    //    if (Int32.TryParse(txtnopage.Text, out result))
    //    {
    //        ViewState["PAGENUMBER"] = Int32.Parse(txtnopage.Text);

    //        if ((int)ViewState["TOTALPAGECOUNT"] < Int32.Parse(txtnopage.Text))
    //            ViewState["PAGENUMBER"] = ViewState["TOTALPAGECOUNT"];

    //        if (0 >= Int32.Parse(txtnopage.Text))
    //            ViewState["PAGENUMBER"] = 1;

    //        if ((int)ViewState["PAGENUMBER"] == 0)
    //            ViewState["PAGENUMBER"] = 1;

    //        txtnopage.Text = ViewState["PAGENUMBER"].ToString();
    //    }
    //    BindData();
    //    SetPageNavigator();
    //}

    //protected void PagerButtonClick(object sender, CommandEventArgs ce)
    //{
    //    gvfundhistory.SelectedIndex = -1;
    //    gvfundhistory.EditIndex = -1;
    //    if (ce.CommandName == "prev")
    //        ViewState["PAGENUMBER"] = (int)ViewState["PAGENUMBER"] - 1;
    //    else
    //        ViewState["PAGENUMBER"] = (int)ViewState["PAGENUMBER"] + 1;

    //    BindData();
    //    SetPageNavigator();
    //}

    //private void SetPageNavigator()
    //{
    //    cmdPrevious.Enabled = IsPreviousEnabled();
    //    cmdNext.Enabled = IsNextEnabled();
    //    lblPagenumber.Text = "Page " + ViewState["PAGENUMBER"].ToString();
    //    lblPages.Text = " of " + ViewState["TOTALPAGECOUNT"].ToString() + " Pages. ";
    //    lblRecords.Text = "(" + ViewState["ROWCOUNT"].ToString() + " records found)";
    //}

    //private Boolean IsPreviousEnabled()
    //{
    //    int iCurrentPageNumber;
    //    int iTotalPageCount;

    //    iCurrentPageNumber = (int)ViewState["PAGENUMBER"];
    //    iTotalPageCount = (int)ViewState["TOTALPAGECOUNT"];

    //    if (iTotalPageCount == 0)
    //        return false;

    //    if (iCurrentPageNumber > 1)
    //        return true;

    //    return false;
    //}

    //private Boolean IsNextEnabled()
    //{
    //    int iCurrentPageNumber;
    //    int iTotalPageCount;

    //    iCurrentPageNumber = (int)ViewState["PAGENUMBER"];
    //    iTotalPageCount = (int)ViewState["TOTALPAGECOUNT"];

    //    if (iCurrentPageNumber < iTotalPageCount)
    //    {
    //        return true;
    //    }
    //    return false;
    //}

   
    protected void gvfundhistory_NeedDataSource(object sender, Telerik.Web.UI.GridNeedDataSourceEventArgs e)
    {
        BindData();
    }
}
