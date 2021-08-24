using System;
using System.Data;
using System.Web.UI;
using System.Web.UI.WebControls;
using SouthNests.Phoenix.Framework;
using SouthNests.Phoenix.Inspection;
using System.Collections.Specialized;
using Telerik.Web.UI;

public partial class InspectionOilMajorCompany : PhoenixBasePage
{
    protected void Page_Load(object sender, EventArgs e)
    {
        SessionUtil.PageAccessRights(this.ViewState);
        PhoenixToolbar toolbar = new PhoenixToolbar();
        try
        {
            toolbar.AddFontAwesomeButton("../Inspection/InspectionOilMajorCompany.aspx", "Export to Excel", "<i class=\"fas fa-file-excel\"></i>", "Excel");
            toolbar.AddFontAwesomeButton("javascript:CallPrint('gvCompany')", "Print Grid", "<i class=\"fas fa-print\"></i>", "PRINT");
            toolbar.AddFontAwesomeButton("../Inspection/InspectionOilMajorCompany.aspx", "Find", "<i class=\"fas fa-search\"></i>", "FIND");            
            MenuCompany.AccessRights = this.ViewState;
            MenuCompany.MenuList = toolbar.Show();

            if (!IsPostBack)
            {
                ViewState["PAGENUMBER"] = 1;
                ViewState["SORTEXPRESSION"] = null;
                ViewState["SORTDIRECTION"] = null;
                ViewState["COMPANYID"] = "";

                NameValueCollection nvc = PhoenixSecurityContext.CurrentSecurityContext.CompanyContext;
                if (nvc.Get("QMS") != null && nvc.Get("QMS") != "")
                {
                    ViewState["COMPANYID"] = nvc.Get("QMS");
                }
                gvCompany.PageSize = int.Parse(PhoenixGeneralSettings.CurrentGeneralSetting.Records);
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
        string[] alColumns = { "FLDSHORTCODE", "FLDOILMAJORCOMPANYNAME", "FLDINSPECTIONNAMELIST" };
        string[] alCaptions = { "Short Code", "Company Name","Inspections" };
        string sortexpression;
        int? sortdirection = null;

        sortexpression = (ViewState["SORTEXPRESSION"] == null) ? null : (ViewState["SORTEXPRESSION"].ToString());
        if (ViewState["SORTDIRECTION"] != null)
            sortdirection = Int32.Parse(ViewState["SORTDIRECTION"].ToString());
        if (ViewState["ROWCOUNT"] == null || Int32.Parse(ViewState["ROWCOUNT"].ToString()) == 0)
            iRowCount = 10;
        else
            iRowCount = Int32.Parse(ViewState["ROWCOUNT"].ToString());

        ds = PhoenixInspectionOilMajorComany.OilMajorCompanySearch(PhoenixSecurityContext.CurrentSecurityContext.UserCode
            , General.GetNullableString(txtShortCode.Text.Trim())
            , General.GetNullableString(txtCompanyName.Text.Trim())
            , sortexpression, sortdirection
            , 1
            , iRowCount
            , ref iRowCount
            , ref iTotalPageCount
            , General.GetNullableInteger(ViewState["COMPANYID"].ToString()));

        Response.AddHeader("Content-Disposition", "attachment; filename=OilMajorCompany.xls");
        Response.ContentType = "application/vnd.msexcel";
        Response.Write("<TABLE BORDER='0' CELLPADDING='2' CELLSPACING='0' width='100%'>");
        Response.Write("<tr>");
        Response.Write("<td><img src='http://" + Request.Url.Authority + Session["images"] + "/esmlogo4_small.png" + "' /></td>");
        Response.Write("<td><h3>OilMajor Company</h3></td>");
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

    protected void MenuCompany_TabStripCommand(object sender, EventArgs e)
    {
        try
        {
            RadToolBarEventArgs dce = (RadToolBarEventArgs)e;
            string CommandName = ((RadToolBarButton)dce.Item).CommandName;
            if (CommandName.ToUpper().Equals("FIND"))
            {
                ViewState["PAGENUMBER"] = 1;
                BindData();
                gvCompany.Rebind();
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

        string[] alColumns = { "FLDSHORTCODE", "FLDOILMAJORCOMPANYNAME", "FLDINSPECTIONNAMELIST" };
        string[] alCaptions = { "Short Code", "Company Name","Inspections" };

        string sortexpression = (ViewState["SORTEXPRESSION"] == null) ? null : (ViewState["SORTEXPRESSION"].ToString());
        int? sortdirection = null;
        DataSet ds = new DataSet();
        if (ViewState["SORTDIRECTION"] != null)
            sortdirection = Int32.Parse(ViewState["SORTDIRECTION"].ToString());

        ds = PhoenixInspectionOilMajorComany.OilMajorCompanySearch(PhoenixSecurityContext.CurrentSecurityContext.UserCode
            , General.GetNullableString(txtShortCode.Text.Trim())
            , General.GetNullableString(txtCompanyName.Text.Trim())
            , sortexpression, sortdirection
            , Int32.Parse(ViewState["PAGENUMBER"].ToString())
            , gvCompany.PageSize
            , ref iRowCount
            , ref iTotalPageCount, General.GetNullableInteger(ViewState["COMPANYID"].ToString()));

        General.SetPrintOptions("gvCompany", "Oil Major Company", alCaptions, alColumns, ds);

        if (ds.Tables[0].Rows.Count > 0)
        {
            gvCompany.DataSource = ds;
            gvCompany.VirtualItemCount = iRowCount;
        }
        else
        {
            gvCompany.DataSource = "";
        }
        ViewState["ROWCOUNT"] = iRowCount;
    }
    
    protected void cmdSearch_Click(object sender, EventArgs e)
    {       
        ViewState["PAGENUMBER"] = 1;
        BindData();
        gvCompany.Rebind();
    }
 
    private void InsertOilMajorCompany(string shortcode, string companyname)
    {
        PhoenixInspectionOilMajorComany.InsertOilMajorCompany(PhoenixSecurityContext.CurrentSecurityContext.UserCode
            , General.GetNullableString(shortcode.Trim())
            , General.GetNullableString(companyname.Trim()));

        ucStatus.Text = "Company added";
    }
    private void UpdateOilMajorCompany(Guid id, string shortcode, string companyname)
    {
        PhoenixInspectionOilMajorComany.UpdateOilMajorCompany(PhoenixSecurityContext.CurrentSecurityContext.UserCode
            , id
            , General.GetNullableString(shortcode.Trim())
            , General.GetNullableString(companyname.Trim()));

        ucStatus.Text = "Information updated";
    }
    private bool IsValidCompany(string shortcode, string companyname)
    {
        ucError.HeaderMessage = "Please provide the following required information";

        if (General.GetNullableString(shortcode) == null)
            ucError.ErrorMessage = "Short Code is required.";

        if (General.GetNullableString(companyname) == null)
            ucError.ErrorMessage = "Company Name is required.";

        return (!ucError.IsError);
    }
    private void DeleteOilMajorCompany(Guid id)
    {
        PhoenixInspectionOilMajorComany.DeleteOilMajorCompany(PhoenixSecurityContext.CurrentSecurityContext.UserCode
            , id);
    }
    public StateBag ReturnViewState()
    {
        return ViewState;
    }

    protected void cmdHiddenSubmit_Click(object sender, EventArgs e)
    {
        BindData();
        gvCompany.Rebind();
    }


    protected void gvCompany_ItemCommand(object sender, GridCommandEventArgs e)
    {
        try
        {
            if (e.CommandName.ToUpper().Equals("SORT"))
                return;
            else if (e.CommandName.ToUpper().Equals("ADD"))
            {
                if (!IsValidCompany(((RadTextBox)e.Item.FindControl("txtShortCodeAdd")).Text
                    , ((RadTextBox)e.Item.FindControl("txtCompanyNameAdd")).Text))
                {
                    ucError.Visible = true;
                    return;
                }
                InsertOilMajorCompany(
                    ((RadTextBox)e.Item.FindControl("txtShortCodeAdd")).Text,
                    ((RadTextBox)e.Item.FindControl("txtCompanyNameAdd")).Text
                );
                BindData();
                gvCompany.Rebind();
            }
            else if (e.CommandName.ToUpper().Equals("SAVE"))
            {
                if (!IsValidCompany(((RadTextBox)e.Item.FindControl("txtShortCodeEdit")).Text
                    , ((RadTextBox)e.Item.FindControl("txtCompanyNameEdit")).Text))
                {
                    ucError.Visible = true;
                    return;
                }
                UpdateOilMajorCompany(
                    new Guid(((RadLabel)e.Item.FindControl("lblOilMajorCompanyIdEdit")).Text),
                     ((RadTextBox)e.Item.FindControl("txtShortCodeEdit")).Text,
                     ((RadTextBox)e.Item.FindControl("txtCompanyNameEdit")).Text
                 );              
                BindData();
                gvCompany.Rebind();
            }

            else if (e.CommandName.ToUpper().Equals("DELETE"))
            {
                DeleteOilMajorCompany(new Guid(((RadLabel)e.Item.FindControl("lblOilMajorCompanyId")).Text));
                BindData();
                gvCompany.Rebind();
            }

            else if (e.CommandName.ToUpper().Equals("UPDATE"))
            {
                if (!IsValidCompany(((RadTextBox)e.Item.FindControl("txtShortCodeEdit")).Text
              , ((RadTextBox)e.Item.FindControl("txtCompanyNameEdit")).Text))
                {
                    ucError.Visible = true;
                    return;
                }

                UpdateOilMajorCompany(
                        new Guid(((RadLabel)e.Item.FindControl("lblOilMajorCompanyIdEdit")).Text),
                        ((RadTextBox)e.Item.FindControl("txtShortCodeEdit")).Text,
                         ((RadTextBox)e.Item.FindControl("txtCompanyNameEdit")).Text
                     );                
                BindData();
                gvCompany.Rebind();
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

    protected void gvCompany_NeedDataSource(object sender, GridNeedDataSourceEventArgs e)
    {
        ViewState["PAGENUMBER"] = ViewState["PAGENUMBER"] != null ? ViewState["PAGENUMBER"] : gvCompany.CurrentPageIndex + 1;
        BindData();
    }

    protected void gvCompany_ItemDataBound(object sender, GridItemEventArgs e)
    {
        if (e.Item is GridDataItem)
        {
            DataRowView drv = (DataRowView)e.Item.DataItem;

            LinkButton db = (LinkButton)e.Item.FindControl("cmdDelete");
            if (db != null)
            {
                db.Attributes.Add("onclick", "return fnConfirmDelete(event); return false;");
                db.Visible = SessionUtil.CanAccess(this.ViewState, db.CommandName);
            }

            LinkButton eb = (LinkButton)e.Item.FindControl("cmdEdit");
            if (eb != null) eb.Visible = SessionUtil.CanAccess(this.ViewState, eb.CommandName);

            LinkButton sb = (LinkButton)e.Item.FindControl("cmdSave");
            if (sb != null) sb.Visible = SessionUtil.CanAccess(this.ViewState, sb.CommandName);

            LinkButton cb = (LinkButton)e.Item.FindControl("cmdCancel");
            LinkButton cmdMap = (LinkButton)e.Item.FindControl("cmdInspectionMapping");

            if (cmdMap != null)
            {
                cmdMap.Visible = SessionUtil.CanAccess(this.ViewState, cmdMap.CommandName);
                cmdMap.Attributes.Add("onclick", "javascript:openNewWindow('codehelp1', '', '" + Session["sitepath"] + "/Inspection/InspectionOilMajorCompanyInspectionMap.aspx?oilmajorcompanyid=" + drv["FLDOILMAJORCOMPANYID"].ToString() + "&oilmajorcompany=" + drv["FLDOILMAJORCOMPANYNAME"].ToString() + "');return false;");
            }
            RadLabel lblInspection = (RadLabel)e.Item.FindControl("lblInspection");
            if (lblInspection != null)
            {
                UserControlToolTip ucToolTipInspection = (UserControlToolTip)e.Item.FindControl("ucToolTipInspection");
                ucToolTipInspection.Position = ToolTipPosition.TopCenter;
                ucToolTipInspection.TargetControlId = lblInspection.ClientID;
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

    protected void gvCompany_SortCommand(object sender, GridSortCommandEventArgs e)
    {
        ViewState["SORTEXPRESSION"] = e.SortExpression;

        if (ViewState["SORTDIRECTION"] != null && ViewState["SORTDIRECTION"].ToString() == "0")
            ViewState["SORTDIRECTION"] = 1;
        else
            ViewState["SORTDIRECTION"] = 0;

        BindData();
    }
}
