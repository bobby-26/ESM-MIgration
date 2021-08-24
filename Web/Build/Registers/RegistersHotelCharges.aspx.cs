using System;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using SouthNests.Phoenix.Framework;
using SouthNests.Phoenix.Registers;
using System.Web.UI.HtmlControls;
using Telerik.Web.UI;


public partial class RegistersHotelCharges : PhoenixBasePage
{
  
    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {
            SessionUtil.PageAccessRights(this.ViewState);
            PhoenixToolbar toolbar = new PhoenixToolbar();
            toolbar.AddFontAwesomeButton("../Registers/RegistersHotelCharges.aspx", "Export to Excel", "<i class=\"fas fa-file-excel\"></i>", "Excel");
            toolbar.AddFontAwesomeButton("javascript:CallPrint('gvHotelCharges')", "Print Grid", "<i class=\"fas fa-print\"></i>", "PRINT");            
            MenuHotelCharges.AccessRights = this.ViewState;
            MenuHotelCharges.MenuList = toolbar.Show();

            toolbar = new PhoenixToolbar();
            MenuTitle.AccessRights = this.ViewState;
            MenuTitle.MenuList = toolbar.Show();
            if (!IsPostBack)
            {
                ViewState["PAGENUMBER"] = 1;
                ViewState["SORTEXPRESSION"] = null;
                ViewState["SORTDIRECTION"] = null;
                ViewState["CURRENTINDEX"] = 1;
                gvHotelCharges.PageSize = int.Parse(PhoenixGeneralSettings.CurrentGeneralSetting.Records);
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
        try
        {
            int iRowCount = 0;
            int iTotalPageCount = 0;

            DataSet ds = new DataSet();
            string[] alColumns = { "FLDCHARGINGSHORTCODE", "FLDCHARGINGNAME", "FLDCOMPANYPAYABLEYN" };
            string[] alCaptions = { "Charging shortcode", "Charging Name", "Compay PayableYN" };

            string sortexpression;
            int? sortdirection = null;

            sortexpression = (ViewState["SORTEXPRESSION"] == null) ? null : (ViewState["SORTEXPRESSION"].ToString());
            if (ViewState["SORTDIRECTION"] != null)
                sortdirection = Int32.Parse(ViewState["SORTDIRECTION"].ToString());

            if (ViewState["ROWCOUNT"] == null || Int32.Parse(ViewState["ROWCOUNT"].ToString()) == 0)
                iRowCount = 10;
            else
                iRowCount = Int32.Parse(ViewState["ROWCOUNT"].ToString());


            ds = PhoenixRegistersHotelCharges.HotelChargesSearch(null
                , null
                , sortexpression, sortdirection,
                (int)ViewState["PAGENUMBER"],
                gvHotelCharges.PageSize,
                ref iRowCount,
                ref iTotalPageCount);

            Response.AddHeader("Content-Disposition", "attachment; filename=HotelCharges.xls");
            Response.ContentType = "application/vnd.msexcel";
            Response.Write("<TABLE BORDER='0' CELLPADDING='2' CELLSPACING='0' width='100%'>");
            Response.Write("<tr>");
            Response.Write("<td><img src='http://" + Request.Url.Authority + Session["images"] + "/esmlogo4_small.png" + "' /></td>");
            Response.Write("<td><h3>Hotel Charges</h3></td>");
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
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }
    protected void MenuHotelCharges_TabStripCommand(object sender, EventArgs e)
    {
        RadToolBarEventArgs dce = (RadToolBarEventArgs)e;
        string CommandName = ((RadToolBarButton)dce.Item).CommandName;
        try
        {
            if (CommandName.ToUpper().Equals("FIND"))
            {
                ViewState["PAGENUMBER"] = 1;
                BindData();
                gvHotelCharges.Rebind();
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

        string[] alColumns = { "FLDCHARGINGSHORTCODE", "FLDCHARGINGNAME", "FLDCOMPANYPAYABLEYN" };
        string[] alCaptions = { "Charging shortcode", "Charging Name", "Compay PayableYN" };

        string sortexpression = (ViewState["SORTEXPRESSION"] == null) ? null : (ViewState["SORTEXPRESSION"].ToString());
        int? sortdirection = null;
        if (ViewState["SORTDIRECTION"] != null)
            sortdirection = Int32.Parse(ViewState["SORTDIRECTION"].ToString());


        DataSet ds = PhoenixRegistersHotelCharges.HotelChargesSearch(null
                 , null
                 , sortexpression, sortdirection
                 , (int)ViewState["PAGENUMBER"]
                 , gvHotelCharges.PageSize
                 , ref iRowCount
                 , ref iTotalPageCount);


        General.SetPrintOptions("gvHotelCharges", "Hotel Charges", alCaptions, alColumns, ds);

        if (ds.Tables[0].Rows.Count > 0)
        {
            gvHotelCharges.DataSource = ds;
            gvHotelCharges.VirtualItemCount = iRowCount;
        }
        else
        {
            gvHotelCharges.DataSource = "";
        }
    }
   
    protected void cmdSearch_Click(object sender, EventArgs e)
    {
        BindData();
        gvHotelCharges.Rebind();
    }
   
    private void InsertHotelCharges(string shortcode, string name, string companypayableYN)
    {
        if (!IsValidHotelCharges(shortcode, name))
        {
            ucError.Visible = true;
            return;
        }
        PhoenixRegistersHotelCharges.InsertHotelCharges(
            PhoenixSecurityContext.CurrentSecurityContext.UserCode
            , shortcode, name, General.GetNullableInteger(companypayableYN));
    }

    private void UpdateHotelCharges(int ChargesId, string shortcode, string name, string companypayableYN)
    {
        if (!IsValidHotelCharges(shortcode, name))
        {
            ucError.Visible = true;
            return;
        }
        PhoenixRegistersHotelCharges.UpdateHotelCharges(
            PhoenixSecurityContext.CurrentSecurityContext.UserCode
            , ChargesId, name, shortcode, General.GetNullableInteger(companypayableYN));
    }

    private bool IsValidHotelCharges(string code, string name)
    {
        ucError.HeaderMessage = "Please provide the following required information";

        if (code.Trim().Equals(""))
            ucError.ErrorMessage = "Shortcode is required.";

        if (name.Trim().Equals(""))
            ucError.ErrorMessage = "Charging name is required.";

        return (!ucError.IsError);
    }

    private void DeleteHotelCharges(int chargesid)
    {
        PhoenixRegistersHotelCharges.DeleteHotelCharges(
            PhoenixSecurityContext.CurrentSecurityContext.UserCode, chargesid);
    }

   
    public StateBag ReturnViewState()
    {
        return ViewState;
    }

    protected void gvHotelCharges_ItemCommand(object sender, GridCommandEventArgs e)
    {
        try
        {
            
            string CompanyPayableYN = String.Empty;

            if (e.CommandName.ToUpper().Equals("ADD"))
            {
                InsertHotelCharges(
                    ((RadTextBox)e.Item.FindControl("txtChargesShortCodeAdd")).Text,
                    ((RadTextBox)e.Item.FindControl("txtChargingNameAdd")).Text,
                    ((RadCheckBox)e.Item.FindControl("chkCompanyPayableAdd")).Checked == true ? "1" : "0"
                );
                BindData();
                gvHotelCharges.Rebind();
            }
            
            else if (e.CommandName.ToUpper().Equals("DELETE"))
            {
                DeleteHotelCharges(Int32.Parse(((RadLabel)e.Item.FindControl("lblChargesId")).Text));
                BindData();
                gvHotelCharges.Rebind();
            }
            else if (e.CommandName.ToUpper().Equals("UPDATE"))
            {
                RadCheckBox CompanyPayable = ((RadCheckBox)e.Item.FindControl("chkCompanyPayableYNEdit"));
                if (CompanyPayable != null)
                {
                    CompanyPayableYN = CompanyPayable.Checked == true ? "1" : "0";
                }
                UpdateHotelCharges(
                         Int32.Parse(((RadLabel)e.Item.FindControl("lblChargesIdEdit")).Text),
                         ((RadTextBox)e.Item.FindControl("txtChargesShortCodeEdit")).Text,
                        ((RadTextBox)e.Item.FindControl("txtChargingNameEdit")).Text,
                        CompanyPayableYN
                     );
                ucStatus.Text = "Information updated";
                BindData();
                gvHotelCharges.Rebind();
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

    protected void gvHotelCharges_NeedDataSource(object sender, GridNeedDataSourceEventArgs e)
    {
        ViewState["PAGENUMBER"] = ViewState["PAGENUMBER"] != null ? ViewState["PAGENUMBER"] : gvHotelCharges.CurrentPageIndex + 1;
        BindData();
    }

    protected void gvHotelCharges_ItemDataBound(object sender, GridItemEventArgs e)
    {
      
        if (e.Item is GridDataItem)
        {
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
            if (cb != null) cb.Visible = SessionUtil.CanAccess(this.ViewState, cb.CommandName);

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

    protected void gvHotelCharges_SortCommand(object sender, GridSortCommandEventArgs e)
    {
        ViewState["SORTEXPRESSION"] = e.SortExpression;

        if (ViewState["SORTDIRECTION"] != null && ViewState["SORTDIRECTION"].ToString() == "0")
            ViewState["SORTDIRECTION"] = 1;
        else
            ViewState["SORTDIRECTION"] = 0;

        BindData();
    }
}
