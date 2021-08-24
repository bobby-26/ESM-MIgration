using System;
using System.Collections;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Xml.Linq;
using SouthNests.Phoenix.Framework;
using SouthNests.Phoenix.OwnerBudget;
using SouthNests.Phoenix.Registers;
using Telerik.Web.UI;


public partial class OwnerBudgetPortOfRotation : PhoenixBasePage
{
    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {
            SessionUtil.PageAccessRights(this.ViewState);

            PhoenixToolbar toolbar = new PhoenixToolbar();
            toolbar.AddFontAwesomeButton("../OwnerBudget/OwnerBudgetPortOfRotation.aspx", "Export to Excel", "<i class=\"fas fa-file-excel\"></i>", "Excel");
            toolbar.AddFontAwesomeButton("javascript:CallPrint('gvContactType')", "Print Grid", "<i class=\"fas fa-print\"></i>", "PRINT");
            MenuInspectionContactType.AccessRights = this.ViewState;
            MenuInspectionContactType.MenuList = toolbar.Show();
            if (!IsPostBack)
            {
                ViewState["PAGENUMBER"] = 1;
                ViewState["SORTEXPRESSION"] = null;
                ViewState["SORTDIRECTION"] = null;
                gvContactType.PageSize = int.Parse(PhoenixGeneralSettings.CurrentGeneralSetting.Records);


                ucNationality.NationalityList = PhoenixRegistersCountry.ListNationality();
                ucNationality.DataBind();
                ucNationality.SelectedNationality = "97";
            }
            BindData();
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

        string[] alColumns = { "ROTATIONPORT", "FLDAIRFARE", "FLDAGENCYFEE" };
        string[] alCaptions = { "Port of Rotation", "Airfare","Agency Fee" };
        string sortexpression;
        int? sortdirection = null;

        sortexpression = (ViewState["SORTEXPRESSION"] == null) ? null : (ViewState["SORTEXPRESSION"].ToString());
        if (ViewState["SORTDIRECTION"] != null)
            sortdirection = Int32.Parse(ViewState["SORTDIRECTION"].ToString());

        if (ViewState["ROWCOUNT"] == null || Int32.Parse(ViewState["ROWCOUNT"].ToString()) == 0)
            iRowCount = 10;
        else
            iRowCount = Int32.Parse(ViewState["ROWCOUNT"].ToString());

        ds = PhoenixOwnerBudgetRegisters.PortOfRotationSearch(General.GetNullableInteger(ucNationality.SelectedNationality)
                       , 1
                       , iRowCount
                       , ref iRowCount
                       , ref iTotalPageCount);


        Response.AddHeader("Content-Disposition", "attachment; filename=Port of Rotation.xls");
        Response.ContentType = "application/vnd.msexcel";
        Response.Write("<TABLE BORDER='0' CELLPADDING='2' CELLSPACING='0' width='100%'>");
        Response.Write("<tr>");
        Response.Write("<td><img src='http://" + Request.Url.Authority + Session["images"] + "/esmlogo4_small.png" + "' /></td>");
        Response.Write("<td><h3>Travel Cost</h3></td>");
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

    protected void InspectionContactType_TabStripCommand(object sender, EventArgs e)
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

    private void BindData()
    {
        int iRowCount = 0;
        int iTotalPageCount = 0;
        string[] alColumns = { "ROTATIONPORT", "FLDAIRFARE", "FLDAGENCYFEE" };
        string[] alCaptions = { "Port of Rotation", "Airfare", "Agency Fee" };

        string sortexpression = (ViewState["SORTEXPRESSION"] == null) ? null : (ViewState["SORTEXPRESSION"].ToString());
        int? sortdirection = null;

        if (ViewState["SORTDIRECTION"] != null)
            sortdirection = Int32.Parse(ViewState["SORTDIRECTION"].ToString());

        DataSet ds = new DataSet();

        ds = PhoenixOwnerBudgetRegisters.PortOfRotationSearch(General.GetNullableInteger(ucNationality.SelectedNationality)
                       , gvContactType.CurrentPageIndex+1
                       , gvContactType.PageSize
                       , ref iRowCount
                       , ref iTotalPageCount);

        General.SetPrintOptions("gvContactType", "Travel Cost", alCaptions, alColumns, ds);
            gvContactType.DataSource = ds;
        gvContactType.VirtualItemCount = iRowCount;

        ViewState["ROWCOUNT"] = iRowCount;


    }
 
    protected void gvContactType_ItemCommand(object sender, GridCommandEventArgs e)
    {
        try
        {
            if (e.CommandName.ToUpper().Equals("SORT"))
                return;


            if (e.CommandName.ToUpper().Equals("ADD"))
            {
                if (!IsValidContactType(ucNationality.SelectedNationality
                                    , ((UserControlQuick)e.Item.FindControl("ucPortofRotaion")).SelectedQuick
                                    , ((UserControlMaskNumber)e.Item.FindControl("ucAirfareAdd")).Text
                                    , ((UserControlMaskNumber)e.Item.FindControl("ucAgencyFeeAdd")).Text
                                    ))
                {
                    ucError.Visible = true;
                    return;
                }
                PhoenixOwnerBudgetRegisters.PortOfRotationInsert(int.Parse(((UserControlQuick)e.Item.FindControl("ucPortofRotaion")).SelectedQuick)
                                                , int.Parse(ucNationality.SelectedNationality)
                                                , decimal.Parse(((UserControlMaskNumber)e.Item.FindControl("ucAirfareAdd")).Text)
                                                , decimal.Parse(((UserControlMaskNumber)e.Item.FindControl("ucAgencyFeeAdd")).Text)
                                                );
            }

            else if (e.CommandName.ToUpper().Equals("UPDATE"))
            {
                if (!IsValidContactType(ucNationality.SelectedNationality
                                    , ((RadLabel)e.Item.FindControl("lblRotationPort")).Text
                                    , ((UserControlMaskNumber)e.Item.FindControl("ucAirfare")).Text
                                    , ((UserControlMaskNumber)e.Item.FindControl("ucAgencyFee")).Text
                                    ))
                {
                    ucError.Visible = true;
                    return;
                }
                PhoenixOwnerBudgetRegisters.PortOfRotationUpdate(new Guid(((RadLabel)e.Item.FindControl("lblPortOfRotationId")).Text.ToString())
                                                           , decimal.Parse(((UserControlMaskNumber)e.Item.FindControl("ucAirfare")).Text)
                                                           , decimal.Parse(((UserControlMaskNumber)e.Item.FindControl("ucAgencyFee")).Text)
                                                            );
            }

            else if (e.CommandName.ToUpper().Equals("DELETE"))
            {
                PhoenixOwnerBudgetRegisters.PortOfRotationDelete(new Guid(((RadLabel)e.Item.FindControl("lblPortOfRotationId")).Text.ToString()));
            }
            BindData();
            gvContactType.Rebind();
        }

        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }

    protected void gvContactType_SortCommand(object sender, GridSortCommandEventArgs e)
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

    protected void gvContactType_ItemDataBound(object sender, GridItemEventArgs e)
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

            {
                LinkButton db = (LinkButton)e.Item.FindControl("cmdDelete");
                if (db != null)
                {
                    db.Attributes.Add("onclick", "return fnConfirmDelete(event); return false;");
                }

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

            UserControlQuick ucPortofRotaion = (UserControlQuick)e.Item.FindControl("ucPortofRotaion");
            if (ucPortofRotaion != null)
            {
                ucPortofRotaion.QuickTypeCode = "115";
                ucPortofRotaion.bind();
            }
            
        }

        
    }
    protected void gvContactType_DeleteCommand(object sender, GridCommandEventArgs e)
    {
        BindData();
    }
    protected void gvContactType_UpdateCommand(object sender, GridCommandEventArgs e)
    {

    }
 

    private bool IsValidContactType(string nationality, string portofrotation, string airfare, string agencyfee)
    {
        ucError.HeaderMessage = "Please provide the following required information";

        if (General.GetNullableInteger(nationality) == null)
            ucError.ErrorMessage = "Nationality is required.";

        if (General.GetNullableInteger(portofrotation) == null)
            ucError.ErrorMessage = "Port of Rotation is required.";

        if (General.GetNullableDecimal(airfare) == null)
            ucError.ErrorMessage = "Airfare is required.";

        if (General.GetNullableDecimal(agencyfee) == null)
            ucError.ErrorMessage = "Agency Fee is required.";

        return (!ucError.IsError);
    }

   



    public StateBag ReturnViewState()
    {
        return ViewState;
    }

    protected void gvContactType_NeedDataSource(object sender, GridNeedDataSourceEventArgs e)
    {
        try
        {
            ViewState["PAGENUMBER"] = ViewState["PAGENUMBER"] != null ? ViewState["PAGENUMBER"] : gvContactType.CurrentPageIndex + 1;
            BindData();
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }
}

