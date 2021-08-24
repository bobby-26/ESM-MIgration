using System;
using System.Data;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using SouthNests.Phoenix.Framework;
using SouthNests.Phoenix.Registers;
using Telerik.Web.UI;

public partial class RegistersDMRAnchorHandlingType : PhoenixBasePage
{
    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {
            SessionUtil.PageAccessRights(this.ViewState);
            PhoenixToolbar toolbar = new PhoenixToolbar();
            toolbar.AddFontAwesomeButton("../Registers/RegistersDMRAnchorHandlingType.aspx", "Export to Excel", "<i class=\"fas fa-file-excel\"></i>", "Excel");
            toolbar.AddFontAwesomeButton("javascript:CallPrint('gvAnchorHandlingType')","Print Grid", "<i class=\"fas fa-print\"></i>", "PRINT");
            toolbar.AddFontAwesomeButton("../Registers/RegistersDMRAnchorHandlingType.aspx", "Find", "<i class=\"fas fa-search\"></i>", "FIND");
           
            MenuRegistersAnchorHandlingType.AccessRights = this.ViewState;
            MenuRegistersAnchorHandlingType.MenuList = toolbar.Show();
            
            if (!IsPostBack)
            {
                ViewState["PAGENUMBER"] = 1;
                ViewState["SORTEXPRESSION"] = null;
                ViewState["SORTDIRECTION"] = null;
                gvAnchorHandlingType.PageSize = int.Parse(PhoenixGeneralSettings.CurrentGeneralSetting.Records);
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
        string[] alColumns = { "FLDANCHORHANDLINGTYPECODE", "FLDANCHORHANDLINGTYPENAME" };
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
        ds = PhoenixRegistersDMRAnchorHandilingType.DMRAnchorHandilingTypeSearch(txtAnchorHandlingType.Text,
                                                            Int32.Parse(ViewState["PAGENUMBER"].ToString()),
                                                            gvAnchorHandlingType.PageSize,
                                                            ref iRowCount,
                                                            ref iTotalPageCount);

        Response.AddHeader("Content-Disposition", "attachment; filename=DMRAnchorHandlingType.xls");
        Response.ContentType = "application/vnd.msexcel";
        Response.Write("<TABLE BORDER='0' CELLPADDING='2' CELLSPACING='0' width='100%'>");
        Response.Write("<tr>");
        Response.Write("<td><img src='http://" + Request.Url.Authority + Session["images"] + "/esmlogo4_small.png" + "' /></td>");
        Response.Write("<td><h3>Anchor Type</h3></td>");
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

            if (CommandName.ToUpper().Equals("FIND"))
            {
                ViewState["PAGENUMBER"] = 1;
                BindData();
                gvAnchorHandlingType.Rebind();
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

        string[] alColumns = { "FLDANCHORHANDLINGTYPECODE", "FLDANCHORHANDLINGTYPENAME" };
        string[] alCaptions = { "Short Code", "Description" };

        string sortexpression = (ViewState["SORTEXPRESSION"] == null) ? null : (ViewState["SORTEXPRESSION"].ToString());
        int? sortdirection = null;
        if (ViewState["SORTDIRECTION"] != null)
            sortdirection = Int32.Parse(ViewState["SORTDIRECTION"].ToString());

        DataSet ds = PhoenixRegistersDMRAnchorHandilingType.DMRAnchorHandilingTypeSearch(txtAnchorHandlingType.Text, 
                                                                    (int)ViewState["PAGENUMBER"],
                                                                    gvAnchorHandlingType.PageSize,
                                                                    ref iRowCount,
                                                                    ref iTotalPageCount);

        General.SetPrintOptions("gvAnchorHandlingType", "Anchor Type", alCaptions, alColumns, ds);

        gvAnchorHandlingType.DataSource = ds;
        gvAnchorHandlingType.VirtualItemCount = iRowCount;

        ViewState["ROWCOUNT"] = iRowCount;
        ViewState["TOTALPAGECOUNT"] = iTotalPageCount;
    }

    //protected void gvAnchorHandlingType_Sorting(object sender, GridViewSortEventArgs se)
    //{
    //    gvAnchorHandlingType.SelectedIndex = -1;
    //    gvAnchorHandlingType.EditIndex = -1;

    //    ViewState["SORTEXPRESSION"] = se.SortExpression;

    //    if (ViewState["SORTDIRECTION"] != null && ViewState["SORTDIRECTION"].ToString() == "0")
    //        ViewState["SORTDIRECTION"] = 1;
    //    else
    //        ViewState["SORTDIRECTION"] = 0;

    //    BindData();
    //}

    protected void gvAnchorHandlingType_ItemCommand(object sender, GridCommandEventArgs e)
    {
       // try
       // {
            if (e.CommandName.ToUpper().Equals("ADD"))
            {
                if (!IsValidAnchorHandlingType(((RadTextBox)e.Item.FindControl("txtAnchorHandlingTypeNameAdd")).Text,
                 ((RadTextBox)e.Item.FindControl("txtAnchorHandlingTypeCodeAdd")).Text))
                {
                    ucError.Visible = true;
                    return;
                }
    
                PhoenixRegistersDMRAnchorHandilingType.InsertDMRAnchorHandilingType(
                    ((RadTextBox)e.Item.FindControl("txtAnchorHandlingTypeNameAdd")).Text,
                    ((RadTextBox)e.Item.FindControl("txtAnchorHandlingTypeCodeAdd")).Text);
                BindData();
                gvAnchorHandlingType.Rebind();
            }
           
            else if (e.CommandName.ToUpper().Equals("DELETE"))
            {
                PhoenixRegistersDMRAnchorHandilingType.DeleteDMRAnchorHandilingType( General.GetNullableGuid (((RadLabel)e.Item.FindControl("lblAnchorHandlingType")).Text));
                BindData();
                gvAnchorHandlingType.Rebind();
            }
            else if (e.CommandName.ToUpper().Equals("EDIT"))
            {
                ((RadLabel)e.Item.FindControl("lbAnchorHandlingType")).Focus();
            }
            else if (e.CommandName.ToUpper().Equals("UPDATE"))
            {
                if (!IsValidAnchorHandlingType(((RadTextBox)e.Item.FindControl("txtAnchorHandlingTypeNameEdit")).Text,
                                               ((RadTextBox)e.Item.FindControl("txtAnchorHandlingTypeIDEdit")).Text)   )
                {
                    ucError.Visible = true;
                    return;
                }
                PhoenixRegistersDMRAnchorHandilingType.UpdateDMRAnchorHandilingType(General.GetNullableGuid(((RadLabel)e.Item.FindControl("lblAnchorHandlingIDEdit")).Text),
                                                                                    ((RadTextBox)e.Item.FindControl("txtAnchorHandlingTypeIDEdit")).Text,
                                                                                    ((RadTextBox)e.Item.FindControl("txtAnchorHandlingTypeNameEdit")).Text);
                
                BindData();
                gvAnchorHandlingType.Rebind();
                ucStatus.Text = "Anchor Handling Type information updated";
            }
            else if (e.CommandName.ToUpper().Equals("PAGE"))
            {
                ViewState["PAGENUMBER"] = null;
            }
        //    }
        //catch (Exception ex)
        //{
        //    ucError.ErrorMessage = ex.Message;
        //    ucError.Visible = true;
        //}
    }
    protected void gvAnchorHandlingType_Sorting(object sender, GridSortCommandEventArgs se)
    {
         ViewState["SORTEXPRESSION"] = se.SortExpression;

        if (ViewState["SORTDIRECTION"] != null && ViewState["SORTDIRECTION"].ToString() == "0")
            ViewState["SORTDIRECTION"] = 1;
        else
            ViewState["SORTDIRECTION"] = 0;

        BindData();
    }

    protected void gvAnchorHandlingType_ItemDataBound(Object sender, GridItemEventArgs e)
    {
    }

    private bool IsValidAnchorHandlingType(string AnchorHandlingTypeName, string AnchorHandlingTypecode)
    {
        ucError.HeaderMessage = "Please provide the following required information";

        if (AnchorHandlingTypecode.Trim().Equals(""))
            ucError.ErrorMessage = "Anchor Handiling Type Code is required.";

        if (AnchorHandlingTypeName.Trim().Equals(""))
            ucError.ErrorMessage = "Anchor Handiling Type Name is required.";


        return (!ucError.IsError);
    }

    protected void gvAnchorHandlingType_NeedDataSource(object sender, Telerik.Web.UI.GridNeedDataSourceEventArgs e)
    {
        try
        {
            ViewState["PAGENUMBER"] = ViewState["PAGENUMBER"] != null ? ViewState["PAGENUMBER"] : gvAnchorHandlingType.CurrentPageIndex + 1;
            BindData();
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }

}
