using System;
using System.Web.UI.WebControls;
using SouthNests.Phoenix.Framework;
using System.Data;
using System.Web.UI.HtmlControls;
using SouthNests.Phoenix.Registers;
using Telerik.Web.UI;

public partial class RegisterCrewAppraisalRemarks : PhoenixBasePage  
{
    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {
            SessionUtil.PageAccessRights(this.ViewState);
            PhoenixToolbar toolbar1 = new PhoenixToolbar();
            toolbar1.AddFontAwesomeButton("../Registers/RegisterCrewAppraisalRemarks.aspx", "Export to Excel", "<i class=\"fas fa-file-excel\"></i>", "Excel");
            toolbar1.AddFontAwesomeButton("javascript:CallPrint('GvCrewAppraisal')", "Print Grid", "<i class=\"fas fa-print\"></i>", "PRINT");            
            MenuCrewAppraisal.AccessRights = this.ViewState;
            MenuCrewAppraisal.MenuList = toolbar1.Show();

            toolbar1 = new PhoenixToolbar();
            MenuTitle.AccessRights = this.ViewState;
            MenuTitle.MenuList = toolbar1.Show();
            if (!IsPostBack)
            {
                ViewState["PAGENUMBER"] = 1;
                ViewState["SORTEXPRESSION"] = null;
                ViewState["SORTDIRECTION"] = null;
                GvCrewAppraisal.PageSize = int.Parse(PhoenixGeneralSettings.CurrentGeneralSetting.Records);
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

        string[] alColumns = { "FLDSORTORDER", "FLDSHORTNAME", "FLDAPPRAISALREMARKS" };
        string[] alCaptions = { "Sort Order", "Short Code", "Appraisal Remarks" };

        string sortexpression = (ViewState["SORTEXPRESSION"] == null) ? null : (ViewState["SORTEXPRESSION"].ToString());
        int? sortdirection = null;

        if (ViewState["SORTDIRECTION"] != null)
            sortdirection = Int32.Parse(ViewState["SORTDIRECTION"].ToString());

        DataSet ds = PhoenixRegistersAppraisalRemarks.SearchAppraisalRemarks(sortexpression
                                                                          , sortdirection
                                                                          , (int)ViewState["PAGENUMBER"]
                                                                          , GvCrewAppraisal.PageSize
                                                                          , ref iRowCount
                                                                          , ref iTotalPageCount
                                                                        );

        General.SetPrintOptions("GvCrewAppraisal", "Appraisal Remarks", alCaptions, alColumns, ds);
        if (ds.Tables[0].Rows.Count > 0)
        {
            GvCrewAppraisal.DataSource = ds;
            GvCrewAppraisal.VirtualItemCount = iRowCount;
        }
        else
        {
            GvCrewAppraisal.DataSource = "";
        }
    }

    protected void MenuCrewAppraisal_TabStripCommand(object sender, EventArgs e) 
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
        int iRowCount = 0;
        int iTotalPageCount = 0;

        string[] alColumns = { "FLDSORTORDER", "FLDSHORTNAME", "FLDAPPRAISALREMARKS" };
        string[] alCaptions = { "Sort Order", "Short Code", "Appraisal Remarks" };

        string sortexpression;
        int? sortdirection = null;

        sortexpression = (ViewState["SORTEXPRESSION"] == null) ? null : (ViewState["SORTEXPRESSION"].ToString());

        if (ViewState["SORTDIRECTION"] != null)
            sortdirection = Int32.Parse(ViewState["SORTDIRECTION"].ToString());
        if (ViewState["ROWCOUNT"] == null || Int32.Parse(ViewState["ROWCOUNT"].ToString()) == 0)
            iRowCount = 10;
        else
            iRowCount = Int32.Parse(ViewState["ROWCOUNT"].ToString());

        DataSet ds = PhoenixRegistersAppraisalRemarks.SearchAppraisalRemarks(sortexpression
                                                                          , sortdirection
                                                                          , (int)ViewState["PAGENUMBER"]
                                                                          , GvCrewAppraisal.PageSize
                                                                          , ref iRowCount
                                                                          , ref iTotalPageCount
                                                                        );

        Response.AddHeader("Content-Disposition", "attachment; filename=CrewAppraisalRemarks.xls");
        Response.ContentType = "application/vnd.msexcel";
        Response.Write("<TABLE BORDER='0' CELLPADDING='2' CELLSPACING='0' width='100%'>");
        Response.Write("<tr>");
        Response.Write("<td><img src='http://" + Request.Url.Authority + Session["images"] + "/esmlogo4_small.png" + "' /></td>");
        Response.Write("<td><h3>Appraisal Remarks</h3></td>");
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
    
    private bool IsValidCrewAppraisal(string sortorder,string shortname, string remarks)
    {
        ucError.HeaderMessage = "Please provide the following required information";
       
        if (General.GetNullableInteger(sortorder) == null)
            ucError.ErrorMessage = "sort order is required.";

        if (shortname.Trim().Equals(""))
            ucError.ErrorMessage = "short name Item is required.";

        if (remarks.Trim().Equals(""))
            ucError.ErrorMessage = "Remark is required.";

        return (!ucError.IsError);

    }
    protected void GvCrewAppraisal_RowCancelingEdit(object sender, GridViewCancelEditEventArgs e) 
    {
        GridView _gridView = (GridView)sender;
        _gridView.EditIndex = -1;
        BindData();
    }

   
    protected void GvCrewAppraisal_ItemCommand(object sender, GridCommandEventArgs e)
    {
        try
        {
            if (e.CommandName.ToUpper().Equals("SORT"))
                return;
            else if (e.CommandName.ToUpper().Equals("ADD"))
            {
                if (!IsValidCrewAppraisal(((UserControlNumber)e.Item.FindControl("ucordersequenceInsert")).Text
                                                                        , ((RadTextBox)e.Item.FindControl("txtShortNameinsert")).Text.Trim()
                                                                        , ((RadTextBox)e.Item.FindControl("txtRemarksinsert")).Text.Trim()))
                {
                    ucError.Visible = true;
                    return;
                }

                PhoenixRegistersAppraisalRemarks.InsertAppraisalRemarks(PhoenixSecurityContext.CurrentSecurityContext.UserCode
                                                                        , int.Parse(((UserControlNumber)e.Item.FindControl("ucordersequenceInsert")).Text)
                                                                        , ((RadTextBox)e.Item.FindControl("txtShortNameinsert")).Text.Trim()
                                                                        , ((RadTextBox)e.Item.FindControl("txtRemarksinsert")).Text.Trim()
                                                                 );

                BindData();
                GvCrewAppraisal.Rebind();                
            }
            else if (e.CommandName.ToUpper().Equals("DELETE"))
            {
                PhoenixRegistersAppraisalRemarks.DeleteAppraisalRemarks(General.GetNullableGuid(((RadLabel)e.Item.FindControl("lblappraisalremarksID")).Text));
                BindData();
                GvCrewAppraisal.Rebind();
            }
            else if (e.CommandName.ToUpper().Equals("UPDATE"))
            {
                if (!IsValidCrewAppraisal(((UserControlNumber)e.Item.FindControl("ucordersequenceEdit")).Text
                                                                      , ((RadTextBox)e.Item.FindControl("txtShortNameedit")).Text.Trim()
                                                                      , ((RadTextBox)e.Item.FindControl("txtRemarksedit")).Text.Trim()))
                {
                    ucError.Visible = true;
                    return;
                }

                PhoenixRegistersAppraisalRemarks.UpdateAppraisalRemarks(PhoenixSecurityContext.CurrentSecurityContext.UserCode,
                                 General.GetNullableGuid(((RadLabel)e.Item.FindControl("lblappraisalremarksEditID")).Text),
                                 int.Parse(((UserControlNumber)e.Item.FindControl("ucordersequenceEdit")).Text),
                                 ((RadTextBox)e.Item.FindControl("txtShortNameedit")).Text.Trim(),
                                 ((RadTextBox)e.Item.FindControl("txtRemarksedit")).Text.Trim()
                                 );
                BindData();
                GvCrewAppraisal.Rebind();
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

    protected void GvCrewAppraisal_NeedDataSource(object sender, GridNeedDataSourceEventArgs e)
    {
        ViewState["PAGENUMBER"] = ViewState["PAGENUMBER"] != null ? ViewState["PAGENUMBER"] : GvCrewAppraisal.CurrentPageIndex + 1;
        BindData();
    }

    protected void GvCrewAppraisal_ItemDataBound(object sender, GridItemEventArgs e)
    {
        if (e.Item is GridDataItem)
        {
            LinkButton db = (LinkButton)e.Item.FindControl("cmdDelete");
            if (db != null) db.Attributes.Add("onclick", "return fnConfirmDelete(event); return false;");
        }
    }
}
