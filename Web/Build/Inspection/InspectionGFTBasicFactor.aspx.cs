using SouthNests.Phoenix.Framework;
using SouthNests.Phoenix.Inspection;
using System;
using System.Data;
using System.Web.UI;
using System.Web.UI.WebControls;
using Telerik.Web.UI;

public partial class InspectionGFTBasicFactor : PhoenixBasePage
{
    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {
            SessionUtil.PageAccessRights(this.ViewState);
            PhoenixToolbar toolbar = new PhoenixToolbar();
            toolbar.AddFontAwesomeButton("../Inspection/InspectionGFTBasicFactor.aspx", "Export to Excel", "<i class=\"fas fa-file-excel\"></i>", "EXCEL");
            toolbar.AddFontAwesomeButton("javascript:CallPrint('gvGFTBasicFactor')", "Print Grid", "<i class=\"fas fa-print\"></i>", "PRINT");
            toolbar.AddFontAwesomeButton("../Inspection/InspectionGFTBasicFactor.aspx", "Find", "<i class=\"fas fa-search\"></i>", "FIND");
            MenuInspectionGFTBasicFactor.AccessRights = this.ViewState;
            MenuInspectionGFTBasicFactor.MenuList = toolbar.Show();

            if (!IsPostBack)
            {
                ViewState["PAGENUMBER"] = 1;
                ViewState["SORTEXPRESSION"] = null;
                ViewState["SORTDIRECTION"] = null;
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

        string[] alColumns = { "FLDSERIALNUMBER", "FLDBASICFACTORNAME" };
        string[] alCaptions = { "S.No", "Basic Factor" };
        string sortexpression;
        int? sortdirection = null;

        sortexpression = (ViewState["SORTEXPRESSION"] == null) ? null : (ViewState["SORTEXPRESSION"].ToString());
        if (ViewState["SORTDIRECTION"] != null)
            sortdirection = Int32.Parse(ViewState["SORTDIRECTION"].ToString());

        if (ViewState["ROWCOUNT"] == null || Int32.Parse(ViewState["ROWCOUNT"].ToString()) == 0)
            iRowCount = 10;
        else
            iRowCount = Int32.Parse(ViewState["ROWCOUNT"].ToString());

        ds = PhoenixInspectionGFTBasicFactor.BasicFactorSearch(
                                                            PhoenixSecurityContext.CurrentSecurityContext.UserCode
                                                            , General.GetNullableString(txtBasicFactorName.Text)
                                                            , sortexpression, sortdirection
                                                            , Int32.Parse(ViewState["PAGENUMBER"].ToString())
                                                            , gvGFTBasicFactor.PageSize
                                                            , ref iRowCount
                                                            , ref iTotalPageCount
                                                            );

        Response.AddHeader("Content-Disposition", "attachment; filename=GFTBasicFactor.xls");
        Response.ContentType = "application/vnd.msexcel";
        Response.Write("<TABLE BORDER='0' CELLPADDING='2' CELLSPACING='0' width='100%'>");
        Response.Write("<tr>");
        Response.Write("<td><img src='http://" + Request.Url.Authority + Session["images"] + "/esmlogo4_small.png" + "' /></td>");
        Response.Write("<td><h3>GFT Basic Factor</h3></td>");
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

    protected void InspectionGFTBasicFactor_TabStripCommand(object sender, EventArgs e)
    {
        try
        {
            RadToolBarEventArgs dce = (RadToolBarEventArgs)e;
            string CommandName = ((RadToolBarButton)dce.Item).CommandName;

            if (CommandName.ToUpper().Equals("FIND"))
            {
                ViewState["PAGENUMBER"] = 1;
                gvGFTBasicFactor.Rebind();
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

    protected void Rebind()
    {
        gvGFTBasicFactor.SelectedIndexes.Clear();
        gvGFTBasicFactor.EditIndexes.Clear();
        gvGFTBasicFactor.DataSource = null;
        gvGFTBasicFactor.Rebind();
    }

    private void BindData()
    {
        int iRowCount = 0;
        int iTotalPageCount = 0;
        string[] alColumns = { "FLDSERIALNUMBER", "FLDBASICFACTORNAME" };
        string[] alCaptions = { "S.No", "Basic Factor" };

        DataSet ds = new DataSet();

        string sortexpression = (ViewState["SORTEXPRESSION"] == null) ? null : (ViewState["SORTEXPRESSION"].ToString());
        int? sortdirection = null;

        if (ViewState["SORTDIRECTION"] != null)
            sortdirection = Int32.Parse(ViewState["SORTDIRECTION"].ToString());

        ds = PhoenixInspectionGFTBasicFactor.BasicFactorSearch(
                                                            PhoenixSecurityContext.CurrentSecurityContext.UserCode
                                                            , General.GetNullableString(txtBasicFactorName.Text)
                                                            , sortexpression, sortdirection
                                                            , gvGFTBasicFactor.CurrentPageIndex + 1
                                                            , gvGFTBasicFactor.PageSize
                                                            , ref iRowCount
                                                            , ref iTotalPageCount
                                                            );

        General.SetPrintOptions("gvGFTBasicFactor", "GFT Basic Factor", alCaptions, alColumns, ds);

        gvGFTBasicFactor.DataSource = ds;
        gvGFTBasicFactor.VirtualItemCount = iRowCount;
    }

    protected void gvGFTBasicFactor_ItemCommand(object sender, GridCommandEventArgs e)
    {
        try
        {
            if (e.CommandName.ToUpper().Equals("ADD"))
            {
                string serialnumber = ((UserControlMaskNumber)e.Item.FindControl("txtSerialNumberAdd")).Text;
                string basicfactorname = ((TextBox)e.Item.FindControl("txtBasicFactorNameAdd")).Text;

                if (!IsValidData(basicfactorname))
                {
                    ucError.Visible = true;
                    return;
                }

                PhoenixInspectionGFTBasicFactor.InsertBasicFactor(PhoenixSecurityContext.CurrentSecurityContext.UserCode,
                    General.GetNullableInteger(serialnumber)
                    , basicfactorname
                    );

                Rebind();
            }

            else if (e.CommandName.ToUpper().Equals("DELETE"))
            {
                GridDataItem item = e.Item as GridDataItem;
                Guid basicfactorid = new Guid(item.GetDataKeyValue("FLDBASICFACTORID").ToString());
                // Guid basicfactorid = new Guid(e.Item.Value.ToString());
                PhoenixInspectionGFTBasicFactor.DeleteBasicFactor(PhoenixSecurityContext.CurrentSecurityContext.UserCode,
                                                basicfactorid);
                Rebind();
            }
            else if (e.CommandName.ToUpper().Equals("UPDATE"))
            {
                string serialnumber = ((UserControlMaskNumber)e.Item.FindControl("txtSerialNumberEdit")).Text;
                string basicfactorname = ((TextBox)e.Item.FindControl("txtBasicFactorNameEdit")).Text;

                if (!IsValidData(basicfactorname))
                {
                    ucError.Visible = true;
                    return;
                }
                GridDataItem item = e.Item as GridDataItem;
                Guid basicfactorid = new Guid(item.GetDataKeyValue("FLDBASICFACTORID").ToString());

                PhoenixInspectionGFTBasicFactor.UpdateBasicFactor(PhoenixSecurityContext.CurrentSecurityContext.UserCode
                                            , basicfactorid
                                            , General.GetNullableInteger(serialnumber)
                                            , basicfactorname
                                           );
                Rebind();
            }
        }

        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }
    protected void gvGFTBasicFactor_ItemDataBound(object sender, GridItemEventArgs e)
    {
        if (e.Item is GridEditableItem)
        {
            LinkButton del = (LinkButton)e.Item.FindControl("cmdDelete");
            if (del != null) del.Visible = SessionUtil.CanAccess(this.ViewState, del.CommandName);

            LinkButton eb = (LinkButton)e.Item.FindControl("cmdEdit");
            if (eb != null) eb.Visible = SessionUtil.CanAccess(this.ViewState, eb.CommandName);

            LinkButton sb = (LinkButton)e.Item.FindControl("cmdSave");
            if (sb != null) sb.Visible = SessionUtil.CanAccess(this.ViewState, sb.CommandName);

            LinkButton cb = (LinkButton)e.Item.FindControl("cmdCancel");
            if (cb != null) cb.Visible = SessionUtil.CanAccess(this.ViewState, cb.CommandName);

                LinkButton db = (LinkButton)e.Item.FindControl("cmdDelete");
                if (db != null)
                {
                    db.Attributes.Add("onclick", "return fnConfirmDelete(event); return false;");
                }
        }
        if (e.Item is GridFooterItem)
        {
            LinkButton db = (LinkButton)e.Item.FindControl("cmdAdd");
            if (db != null)
            {
                if (!SessionUtil.CanAccess(this.ViewState, db.CommandName))
                    db.Visible = false;
            }
        }
    }

    private bool IsValidData(string basicfactor)
    {
        ucError.HeaderMessage = "Please provide the following required information";

        if (basicfactor.Equals(""))
            ucError.ErrorMessage = "Basic Factor is required.";

        return (!ucError.IsError);

    }

    public StateBag ReturnViewState()
    {
        return ViewState;
    }


    protected void gvGFTBasicFactor_NeedDataSource(object sender, Telerik.Web.UI.GridNeedDataSourceEventArgs e)
    {
        BindData();
    }
}

