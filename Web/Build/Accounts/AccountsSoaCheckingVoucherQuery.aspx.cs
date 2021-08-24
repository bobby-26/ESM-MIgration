using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.HtmlControls;
using System.Data;
using SouthNests.Phoenix.Accounts;
using SouthNests.Phoenix.Framework;
using SouthNests.Phoenix.Registers;
using SouthNests.Phoenix.Common;
using Telerik.Web.UI;
public partial class Accounts_AccountsSoaCheckingVoucherQuery : PhoenixBasePage
{
    
    protected void Page_Load(object sender, EventArgs e)
     {
        SessionUtil.PageAccessRights(this.ViewState);
        cmdHiddenSubmit.Attributes.Add("style", "display:none;");
        if (!IsPostBack)
        {
            ViewState["PAGENUMBER"] = 1;
            ViewState["SORTEXPRESSION"] = null;
            ViewState["SORTDIRECTION"] = null;


            ViewState["PAGENUMBERSECTION"] = 1;
            ViewState["SORTEXPRESSIONSECTION"] = null;
            ViewState["SORTDIRECTIONSECTION"] = null;

            if (Request.QueryString["voucherid"].ToString() != null)
                ViewState["voucherid"] = Request.QueryString["voucherid"].ToString();
            else
                ViewState["voucherid"] = "";

            gvOperations.PageSize = int.Parse(PhoenixGeneralSettings.CurrentGeneralSetting.Records);
            gvDeck.PageSize = int.Parse(PhoenixGeneralSettings.CurrentGeneralSetting.Records);

        }
       
        //BindAnswers();
        //SetPageNavigatorDeck();

    }

    private void BindQuestion()
    {
        int iRowCount = 0;
        int iTotalPageCount = 0;

        string sortexpression = (ViewState["SORTEXPRESSION"] == null) ? null : (ViewState["SORTEXPRESSION"].ToString());
        int? sortdirection = null;

        if (ViewState["SORTDIRECTION"] != null)
            sortdirection = Int32.Parse(ViewState["SORTDIRECTION"].ToString());

         if (ViewState["ROWCOUNT"] == null || Int32.Parse(ViewState["ROWCOUNT"].ToString()) == 0)
            iRowCount = 10;
        else
            iRowCount = Int32.Parse(ViewState["ROWCOUNT"].ToString());

        DataSet ds = new DataSet();

        ds = PhoenixAccountsSoaChecking.SoaQuerySearch(General.GetNullableGuid(ViewState["voucherid"].ToString()), (int)ViewState["PAGENUMBER"],
                                                 gvOperations.PageSize,
                                                 ref iRowCount,
                                                 ref iTotalPageCount);
        gvOperations.DataSource = ds;
        if (ds.Tables[0].Rows.Count > 0)
        {
           
         
            if (!IsPostBack)
            {               
                ViewState["questionid"] = ds.Tables[0].Rows[0]["FLDQUERYID"].ToString();
            }

        }
        else
        {
            DataTable dt = ds.Tables[0];
            ViewState["questionid"] = "";
           
        }
        gvOperations.VirtualItemCount = iRowCount;
        ViewState["ROWCOUNT"] = iRowCount;
        ViewState["TOTALPAGECOUNT"] = iTotalPageCount;
    }

    protected void gvOperations_Sorting(object sender, GridViewSortEventArgs se)
    {
      
        ViewState["SORTEXPRESSION"] = se.SortExpression;

        if (ViewState["SORTDIRECTION"] != null && ViewState["SORTDIRECTION"].ToString() == "0")
            ViewState["SORTDIRECTION"] = 1;
        else
            ViewState["SORTDIRECTION"] = 0;

    }

  

    //protected void gvOperations_SelectedIndexChanging(object sender, GridViewSelectEventArgs se)
    //{
    //    gvDeck.SelectedIndex = se.NewSelectedIndex;

    //    ViewState["questionid"] = ((RadLabel)gvOperations.Items[se.NewSelectedIndex].FindControl("lblQueryId")).Text;

    //    BindAnswers();
    //    SetPageNavigatorDeck();
    //}

    protected void cmdSearch_Click(object sender, EventArgs e)
    {     
        BindQuestion();        
    }

  

    public StateBag ReturnViewState()
    {
        return ViewState;
    }
    private void BindAnswers()
    {
        int iRowCount = 0;
        int iTotalPageCount = 0;

        string sortexpression = (ViewState["SORTEXPRESSIONSECTION"] == null) ? null : (ViewState["SORTEXPRESSIONSECTION"].ToString());
        int? sortdirection = null;

        if (ViewState["SORTDIRECTIONSECTION"] != null)
            sortdirection = Int32.Parse(ViewState["SORTDIRECTIONSECTION"].ToString());

        if (ViewState["ROWCOUNT"] == null || Int32.Parse(ViewState["ROWCOUNT"].ToString()) == 0)
            iRowCount = 10;
        else
            iRowCount = Int32.Parse(ViewState["ROWCOUNT"].ToString());

        DataSet ds = new DataSet();

        ds = PhoenixAccountsSoaChecking.SoaCheckingAnswerSearch(General.GetNullableGuid(ViewState["questionid"].ToString()), (int)ViewState["PAGENUMBERSECTION"],
                                                 gvDeck.PageSize,
                                                 ref iRowCount,
                                                 ref iTotalPageCount);
        gvDeck.DataSource = ds;
        gvDeck.VirtualItemCount = iRowCount;

        ViewState["ROWCOUNTSECTION"] = iRowCount;
        ViewState["TOTALPAGECOUNTSECTION"] = iTotalPageCount;
    }

    private void ShowNoRecordsFoundDeck(DataTable dt, GridView gv)
    {
        dt.Rows.Add(dt.NewRow());
        gv.DataSource = dt;
        gv.DataBind();

        int colcount = gv.Columns.Count;
        gv.Rows[0].Cells.Clear();
        gv.Rows[0].Cells.Add(new TableCell());
        gv.Rows[0].Cells[0].ColumnSpan = colcount;
        gv.Rows[0].Cells[0].HorizontalAlign = HorizontalAlign.Center;
        gv.Rows[0].Cells[0].ForeColor = System.Drawing.Color.Red;
        gv.Rows[0].Cells[0].Font.Bold = true;
        gv.Rows[0].Cells[0].Text = "NO RECORDS FOUND";
        gv.Rows[0].Attributes["onclick"] = "";
    }


    protected void cmdHiddenSubmit_Click(object sender, EventArgs e)
    {
        BindQuestion();
        gvOperations.Rebind();
        BindAnswers();
        gvDeck.Rebind();
    }

    protected void gvOperations_NeedDataSource(object sender, Telerik.Web.UI.GridNeedDataSourceEventArgs e)
    {
        try
        {
            ViewState["PAGENUMBER"] = ViewState["PAGENUMBER"] != null ? ViewState["PAGENUMBER"] : gvOperations.CurrentPageIndex + 1;
            BindQuestion();
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }

    protected void gvOperations_ItemCommand(object sender, Telerik.Web.UI.GridCommandEventArgs e)
    {
        try
        {
            if (e.CommandName.ToUpper().Equals("SORT"))
                return;

          

            if (e.CommandName.ToUpper().Equals("OPERATIONSADD"))
            {
                string description = ((RadTextBox)e.Item.FindControl("txtDescriptionAdd")).Text;

                PhoenixAccountsSoaChecking.SoaCheckingQueryInsert(PhoenixSecurityContext.CurrentSecurityContext.UserCode, description, General.GetNullableGuid(ViewState["voucherid"].ToString()));

                BindQuestion();
                gvOperations.Rebind();

            }
            else if (e.CommandName.ToUpper().Equals("UPDATE"))
            {
                BindQuestion();
                gvOperations.Rebind();

            }                      
            else if (e.CommandName.ToUpper().Equals("SELECT"))
            {
                ViewState["questionid"] = ((RadLabel)e.Item.FindControl("lblQueryId")).Text;
                BindAnswers();
                gvDeck.Rebind();
            }

            else if (e.CommandName.ToUpper().Equals("CANCELTCKT"))
            {
                RadLabel lblQueryId = (RadLabel)e.Item.FindControl("lblQueryId");

                PhoenixAccountsSoaChecking.QueryCancel(PhoenixSecurityContext.CurrentSecurityContext.UserCode, General.GetNullableGuid(lblQueryId.Text));

                BindQuestion();
                gvOperations.Rebind();
                BindAnswers();
                gvDeck.Rebind();
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

    protected void gvOperations_ItemCreated(object sender, Telerik.Web.UI.GridItemEventArgs e)
    {
        if (e.Item is GridDataItem
          && !e.Item.Equals(DataControlRowState.Edit | DataControlRowState.Selected | DataControlRowState.Alternate)
          && !e.Item.Equals(DataControlRowState.Edit | DataControlRowState.Selected | DataControlRowState.Normal))
        {
            e.Item.TabIndex = -1;
            e.Item.Attributes["onclick"] = Page.ClientScript.GetPostBackEventReference(gvOperations, "Select$" + e.Item.RowIndex.ToString(), false);
        }
    }

    protected void gvDeck_NeedDataSource(object sender, GridNeedDataSourceEventArgs e)
    {
        try
        {
            ViewState["PAGENUMBERSECTION"] = ViewState["PAGENUMBERSECTION"] != null ? ViewState["PAGENUMBERSECTION"] : gvDeck.CurrentPageIndex + 1;
            BindAnswers();
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }

    protected void gvDeck_ItemCommand(object sender, GridCommandEventArgs e)
    {
        try
        {
            if (e.CommandName.ToUpper().Equals("SORT"))
                return;

           if (e.CommandName.ToUpper().Equals("UPDATE"))
            {
                string description = ((RadTextBox)e.Item.FindControl("txtDescriptionEdit")).Text;

                PhoenixAccountsSoaChecking.QueryAnswerUpdate(PhoenixSecurityContext.CurrentSecurityContext.UserCode, General.GetNullableGuid(((RadLabel)e.Item.FindControl("lblQueryAnswerIdEdit")).Text), description);
                BindQuestion();
                gvOperations.Rebind();
                BindAnswers();
                gvDeck.Rebind();
            }
            else if (e.CommandName.ToUpper().Equals("DECKADD"))
            {
                if (ViewState["questionid"].ToString() == "")
                {
                    ucError.ErrorMessage = "Select a question to enter the answer";
                    ucError.Visible = true;
                    return;
                }

                string description = ((RadTextBox)e.Item.FindControl("txtDescriptionAdd")).Text;

                if (string.IsNullOrEmpty(description))
                {
                    ucError.ErrorMessage = "Answer is required.";
                    ucError.Visible = true;
                    return;
                }

                PhoenixAccountsSoaChecking.QueryAnswersInsert(PhoenixSecurityContext.CurrentSecurityContext.UserCode, General.GetNullableGuid(ViewState["questionid"].ToString()), description);

                BindQuestion();
                gvOperations.Rebind();
                BindAnswers();
                gvDeck.Rebind();
            }
            else if (e.CommandName.ToUpper().Equals("CLOSE"))
            {
                PhoenixAccountsSoaChecking.SoaCheckingQueryAnswerClose(PhoenixSecurityContext.CurrentSecurityContext.UserCode, General.GetNullableGuid(((RadLabel)e.Item.FindControl("lblQueryAnswerId")).Text));

                BindQuestion();
                gvOperations.Rebind();
                BindAnswers();
                gvDeck.Rebind();
            }

            else if (e.CommandName.ToUpper().Equals("REOPEN"))
            {
                string lblquestionanswerid = ((RadLabel)e.Item.FindControl("lblQueryAnswerId")).Text;
                string pgname = "~/Accounts/AccountsSoaCheckingVoucherQuery.aspx";

                Response.Redirect("AccountsSoaCheckingReopenRemarks.aspx?questionanswerid=" + lblquestionanswerid.ToString() + "&redirect=" + pgname.ToString() + "&voucherid=" + ViewState["voucherid"].ToString());
            }
            else if (e.CommandName == "Page")
            {
                ViewState["PAGENUMBERSECTION"] = null;
            }

            
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;

        }
    }

    protected void gvOperations_ItemDataBound1(object sender, GridItemEventArgs e)
    {
        

        if (e.Item is GridDataItem)
        {
            DataRowView drv = (DataRowView)e.Item.DataItem;

            ImageButton imgQueryCancel = (ImageButton)e.Item.FindControl("cmdCancelquery");

            if (drv != null && drv["FLDCREATEDBY"].ToString() != "")
            {
                if (PhoenixSecurityContext.CurrentSecurityContext.UserCode == int.Parse(drv["FLDCREATEDBY"].ToString()))
                {
                    if (imgQueryCancel != null)
                    {
                        if (drv["FLDSTATUS"].ToString() == PhoenixCommonRegisters.GetHardCode(1, 235, "SOP"))
                            imgQueryCancel.Visible = true;
                    }
                }

                if (drv["FLDSTATUS"].ToString() != PhoenixCommonRegisters.GetHardCode(1, 235, "SOP"))
                {
                    ImageButton imgEdit = (ImageButton)e.Item.FindControl("cmdEdit");
                    if (imgEdit != null)
                        imgEdit.Visible = false;
                }

            }
        }

    }

    protected void gvDeck_ItemCreated(object sender, GridItemEventArgs e)
    {

        if (e.Item is GridDataItem
            && !e.Item.Equals(DataControlRowState.Edit | DataControlRowState.Selected | DataControlRowState.Alternate)
            && !e.Item.Equals(DataControlRowState.Edit | DataControlRowState.Selected | DataControlRowState.Normal))
        {
            e.Item.TabIndex = -1;
            e.Item.Attributes["onclick"] = Page.ClientScript.GetPostBackEventReference(gvDeck, "Select$" + e.Item.RowIndex.ToString(), false);
        }
    }

    protected void gvDeck_ItemDataBound(object sender, GridItemEventArgs e)
     {
        
        if (e.Item is GridDataItem)
        {
            ImageButton imgreopen = (ImageButton)e.Item.FindControl("cmdReopen");
            ImageButton imgclose = (ImageButton)e.Item.FindControl("cmdClose");
            ImageButton imgEdit = (ImageButton)e.Item.FindControl("cmdEdit");

            DataRowView drv = (DataRowView)e.Item.DataItem;

            if (drv != null && drv["FLDQUESTIONSTATUS"].ToString() == PhoenixCommonRegisters.GetHardCode(1, 235, "SCD"))
            {
                imgreopen.Visible = false;
                imgclose.Visible = false;
                imgEdit.Visible = false;
            }

            if (drv != null && drv["FLDANSWERSTATUS"].ToString() == PhoenixCommonRegisters.GetHardCode(1, 235, "SRO"))
            {
                imgreopen.Visible = false;
                imgclose.Visible = false;
                imgEdit.Visible = false;
            }

            //if (drv != null && drv["FLDCLOSED"].ToString() == "1" && imgreopen != null)
            //    imgreopen.Visible = false;

            RadLabel lblquestionanswerid = (RadLabel)e.Item.FindControl("lblQueryAnswerId");

            if (imgreopen != null)
            {
                // imgreopen.Attributes.Add("onclick", "javascript:Openpopup('Filter','','AccountsSoaCheckingReopenRemarks.aspx?questionanswerid=" + lblquestionanswerid.Text +"&redirect="+pgname.ToString()+"&voucherid="+ViewState["voucherid"].ToString()+ "'); return false;");                              
            }

        }

       
    }
}
