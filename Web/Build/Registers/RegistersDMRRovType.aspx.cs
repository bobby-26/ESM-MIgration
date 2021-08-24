using System;
using System.Data;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using SouthNests.Phoenix.Framework;
using SouthNests.Phoenix.Registers;
using Telerik.Web.UI;

public partial class RegistersDMRRovType : PhoenixBasePage
{
    //protected void gvDMRRowType_SelectedIndexChanging(object sender, gridindex e)
    //{
    //    gvDMRRowType.SelectedIndex = e.NewSelectedIndex;
    //}
    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {
            SessionUtil.PageAccessRights(this.ViewState);
            PhoenixToolbar toolbar = new PhoenixToolbar();

            toolbar.AddFontAwesomeButton("../Registers/RegistersDMRRovType.aspx", "Export to Excel", "<i class=\"fas fa-file-excel\"></i>", "Excel");
            toolbar.AddFontAwesomeButton("javascript:CallPrint('gvDMRRovType')", "Print Grid","<i class=\"fas fa-print\"></i>", "PRINT");
            toolbar.AddFontAwesomeButton("../Registers/RegistersDMRRovType.aspx", "Find","<i class=\"fas fa-search\"></i>", "FIND");
            
            MenuRegistersDMRRovType.AccessRights = this.ViewState;
            MenuRegistersDMRRovType.MenuList = toolbar.Show();
            
            if (!IsPostBack)
            {
                ViewState["PAGENUMBER"] = 1;
                ViewState["SORTEXPRESSION"] = null;
                ViewState["SORTDIRECTION"] = null;
                gvDMRRovType.PageSize = int.Parse(PhoenixGeneralSettings.CurrentGeneralSetting.Records);
                //BindData();
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
        string[] alColumns = { "FLDDMRROVTYPECODE", "FLDDMRROVTYPENAME" };
        string[] alCaptions = { "Short Code", "Description" };
        string sortexpression;
        int? sortdirection = null;

        sortexpression = (ViewState["SORTEXPRESSION"] == null) ? null : (ViewState["SORTEXPRESSION"].ToString());
        if (ViewState["SORTDIRECTION"] != null)
            sortdirection = Int32.Parse(ViewState["SORTDIRECTION"].ToString());
        if (ViewState["ROWCOUNT"] == null || Int32.Parse(ViewState["ROWCOUNT"].ToString()) == 0)
            iRowCount = 10;
        else
            iRowCount = Int32.Parse(ViewState["ROWCOUNT"].ToString());
        ds = PhoenixRegistersDMRRovType.MDRRovTypeSearch(txtDMRRowType.Text,
                                                            Int32.Parse(ViewState["PAGENUMBER"].ToString()),
                                                            gvDMRRovType.PageSize,
                                                            ref iRowCount,
                                                            ref iTotalPageCount);

        Response.AddHeader("Content-Disposition", "attachment; filename=DMRRovType.xls");
        Response.ContentType = "application/vnd.msexcel";
        Response.Write("<TABLE BORDER='0' CELLPADDING='2' CELLSPACING='0' width='100%'>");
        Response.Write("<tr>");
        Response.Write("<td><img src='http://" + Request.Url.Authority + Session["images"] + "/esmlogo4_small.png" + "' /></td>");
        Response.Write("<td><h3>ROV Class</h3></td>");
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

    protected void RegistersDMRRovType_TabStripCommand(object sender, EventArgs e)
    {
        try
        {
            RadToolBarEventArgs dce = (RadToolBarEventArgs)e;
            string CommandName = ((RadToolBarButton)dce.Item).CommandName;

            //string a;
            //a = ViewState["PAGENUMBER"].ToString();

            if (CommandName.ToUpper().Equals("FIND"))
            {
                ViewState["PAGENUMBER"] = 1;
                //gvDMRRovType.PageSize = int.Parse(PhoenixGeneralSettings.CurrentGeneralSetting.Records);
                BindData();
                gvDMRRovType.Rebind();
            }
            
            //string b;
            //b = ViewState["PAGENUMBER"].ToString();
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

        string[] alColumns = { "FLDDMRROVTYPECODE", "FLDDMRROVTYPENAME" };
        string[] alCaptions = { "Short Code", "Description" };

        string sortexpression = (ViewState["SORTEXPRESSION"] == null) ? null : (ViewState["SORTEXPRESSION"].ToString());
        int? sortdirection = null;
        if (ViewState["SORTDIRECTION"] != null)
            sortdirection = Int32.Parse(ViewState["SORTDIRECTION"].ToString());

        DataSet ds = PhoenixRegistersDMRRovType.MDRRovTypeSearch( txtDMRRowType.Text, 
                                                                    (int)ViewState["PAGENUMBER"],
                                                                    gvDMRRovType.PageSize,
                                                                    ref iRowCount,
                                                                    ref iTotalPageCount);

        General.SetPrintOptions("gvDMRRovType", "ROV Class", alCaptions, alColumns, ds);

        gvDMRRovType.DataSource = ds;
        gvDMRRovType.VirtualItemCount = iRowCount;

        ViewState["ROWCOUNT"] = iRowCount;
        ViewState["TOTALPAGECOUNT"] = iTotalPageCount;
    }

    //protected void gvDMRRowType_Sorting(object sender, GridViewSortEventArgs se)
    //{
    //    gvDMRRowType.SelectedIndex = -1;
    //    gvDMRRowType.EditIndex = -1;

    //    ViewState["SORTEXPRESSION"] = se.SortExpression;

    //    if (ViewState["SORTDIRECTION"] != null && ViewState["SORTDIRECTION"].ToString() == "0")
    //        ViewState["SORTDIRECTION"] = 1;
    //    else
    //        ViewState["SORTDIRECTION"] = 0;

    //    BindData();
    //}



    //protected void aa(object sender, GridCommandEventArgs de)
    //{
    //    try
    //    {
    //        GridView _gridView = (GridView)sender;

    //        _gridView.EditIndex = de.NewEditIndex;
    //        _gridView.SelectedIndex = de.NewEditIndex;

    //        BindData();
    //        ((TextBox)_gridView.Rows[de.NewEditIndex].FindControl("txtDMRRovTypeNameEdit")).Focus();
    //    }
    //    catch (Exception ex)
    //    {
    //        ucError.ErrorMessage = ex.Message;
    //        ucError.Visible = true;
    //    }
    //}

    protected void gvDMRRovType_ItemCommand(object sender, GridCommandEventArgs e)
    {
        try
        {
            if (e.CommandName.ToUpper().Equals("ADD"))
            {
                if (!IsValidDMRRovType(((RadTextBox)e.Item.FindControl("txtDMRRovTypeNameAdd")).Text,
                 ((RadTextBox)e.Item.FindControl("txtDMRRovTypeCodeAdd")).Text))
                {
                    ucError.Visible = true;
                }
                else
                {
                    PhoenixRegistersDMRRovType.InsertMDRRovType(
                        ((RadTextBox)e.Item.FindControl("txtDMRRovTypeNameAdd")).Text,
                        ((RadTextBox)e.Item.FindControl("txtDMRRovTypeCodeAdd")).Text);
                }
   
                BindData();
                gvDMRRovType.Rebind();
            }
            else if (e.CommandName.ToUpper().Equals("PAGE"))
            {
                ViewState["PAGENUMBER"] = null;
            }
            else if (e.CommandName.ToUpper().Equals("DELETE"))
            {
                PhoenixRegistersDMRRovType.DeleteMDRRovType(General.GetNullableGuid(((RadLabel)e.Item.FindControl("lblDMRRovTypeID")).Text));
                BindData();
                gvDMRRovType.Rebind();
            }

            else if (e.CommandName.ToUpper().Equals("UPDATE"))
            {
                if (!IsValidDMRRovType(((RadTextBox)e.Item.FindControl("txtDMRRovTypeNameEdit")).Text,
                   ((RadTextBox)e.Item.FindControl("txtDMRRovTypeIDDEditEdit")).Text))
                {
                    ucError.Visible = true;
                    return;
                }
                PhoenixRegistersDMRRovType.UpdateMDRRovType(General.GetNullableGuid(((RadLabel)e.Item.FindControl("lblDMRRovTypeIDDEdit")).Text),
                              ((RadTextBox)e.Item.FindControl("txtDMRRovTypeIDDEditEdit")).Text,
                              ((RadTextBox)e.Item.FindControl("txtDMRRovTypeNameEdit")).Text);
                ucStatus.Text = "Rov Type information updated";

                BindData();
                gvDMRRovType.Rebind();
            }
            else if (e.CommandName.ToUpper().Equals("EDIT"))
            {
                ((RadLabel)e.Item.FindControl("lbDMRRovType")).Focus();
            }
            
            }
        
        
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
       
    }
    protected void gvDMRRovType_Sorting(object sender, GridSortCommandEventArgs se)
    {
        
        ViewState["SORTEXPRESSION"] = se.SortExpression;

        if (ViewState["SORTDIRECTION"] != null && ViewState["SORTDIRECTION"].ToString() == "0")
            ViewState["SORTDIRECTION"] = 1;
        else
            ViewState["SORTDIRECTION"] = 0;

        BindData();
    }

    protected void gvDMRRovType_ItemDataBound(Object sender, GridItemEventArgs e)
    {
    }

    private bool IsValidDMRRovType(string DMRRowTypeName, string DMRRowTypecode)
    {
        ucError.HeaderMessage = "Please provide the following required information";

        if (DMRRowTypecode.Trim().Equals(""))
            ucError.ErrorMessage = "Rov Type Code is required.";

        if (DMRRowTypeName.Trim().Equals(""))
            ucError.ErrorMessage = "Rov Type Name is required.";


        return (!ucError.IsError);
    }

    protected void gvDMRRovType_NeedDataSource(object sender, Telerik.Web.UI.GridNeedDataSourceEventArgs e)
    {
        try
        {
            ViewState["PAGENUMBER"] = ViewState["PAGENUMBER"] != null ? ViewState["PAGENUMBER"] : gvDMRRovType.CurrentPageIndex + 1;
            BindData();
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }
}
