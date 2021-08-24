using System;
using System.Data;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using SouthNests.Phoenix.Framework;
using SouthNests.Phoenix.Registers;
using SouthNests.Phoenix.Common;
using SouthNests.Phoenix.VesselPosition;
using Telerik.Web.UI;

public partial class VesselPositionBunkerReceiptList : PhoenixBasePage
{
    int ifueloil = 0;
    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {            

            PhoenixToolbar toolbartap = new PhoenixToolbar();
            toolbartap.AddButton("ESI", "ESI");
            toolbartap.AddButton("Quarterly EEOI", "EEOI");
            toolbartap.AddButton("BDN", "BDN");
            toolbartap.AddButton("Chart", "CHART");
            MenuBunkerReceipt.AccessRights = this.ViewState;
            MenuBunkerReceipt.MenuList = toolbartap.Show();
            MenuBunkerReceipt.SelectedMenuIndex = 2; 

            if (!IsPostBack)
            {
                ViewState["PAGENUMBER"] = 1;
                ViewState["SORTEXPRESSION"] = null;
                ViewState["SORTDIRECTION"] = null;
                ViewState["VESSELID"] = "";
                BindOil();
                rblFuelOilType.SelectedIndex = 0;
                if (rblFuelOilType.SelectedIndex != -1)
                {
                    ViewState["OILTYPE"] = rblFuelOilType.Items[0].Value.Trim();
                }
                else
                {
                    ViewState["OILTYPE"] = "";
                }

                if (PhoenixSecurityContext.CurrentSecurityContext.VesselID != 0)
                {
                    UcVessel.SelectedVessel = PhoenixSecurityContext.CurrentSecurityContext.VesselID.ToString();
                    UcVessel.Enabled = false;
                }
                gvFuelOil.PageSize = int.Parse(PhoenixGeneralSettings.CurrentGeneralSetting.Records);
            }
        }
        catch (Exception ex)
        {
            ucError.HeaderMessage = "";
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }

    protected void MenuBunkerReceipt_TabStripCommand(object sender, EventArgs e)
    {
        RadToolBarEventArgs dce = (RadToolBarEventArgs)e;
        string CommandName = ((RadToolBarButton)dce.Item).CommandName;

        if (CommandName.ToUpper().Equals("ESI"))
            Response.Redirect("../VesselPosition/VesselPositionESIRegister.aspx");

        if (CommandName.ToUpper().Equals("CHART"))
            Response.Redirect("../VesselPosition/VesselPositionESIChart.aspx");
        if (CommandName.ToUpper().Equals("EEOI"))
        {
            Response.Redirect("../VesselPosition/vesselpositionyeartodatequaterreport.aspx");
        }
    }

    protected void SetFuelOilValue(object sender, EventArgs e)
    {
        try
        {
            rblLubeOilType.SelectedIndex = -1;
            ViewState["OILTYPE"] = rblFuelOilType.SelectedValue;
            Rebind();
        }
        catch (Exception ex)
        {
            ucError.HeaderMessage = "";
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }

    protected void SetLubeOilValue(object sender, EventArgs e)
    {
        try
        {
            rblFuelOilType.SelectedIndex = -1;
            ViewState["OILTYPE"] = rblLubeOilType.SelectedValue;
            Rebind();
        }
        catch (Exception ex)
        {
            ucError.HeaderMessage = "";
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }

    private void BindOil()
    {
        rblFuelOilType.DataSource = PhoenixVesselPositionBunkerReceipt.OilList(1, 1);
        rblFuelOilType.DataBindings.DataTextField = "FLDSHORTNAME";
        rblFuelOilType.DataBindings.DataValueField = "FLDOILTYPECODE";
        rblFuelOilType.DataBind();

        rblLubeOilType.DataSource = PhoenixVesselPositionBunkerReceipt.OilList(1, 0);
        rblLubeOilType.DataBindings.DataTextField = "FLDSHORTNAME";
        rblLubeOilType.DataBindings.DataValueField = "FLDOILTYPECODE";
        rblLubeOilType.DataBind();
    }
    protected void gvFuelOil_ItemDataBound(Object sender, GridItemEventArgs e)
    {
        try
        {
            if(e.Item is GridHeaderItem)
            {
                if (ifueloil == 0 )
                {
                    gvFuelOil.MasterTableView.GetColumn("Dencity").Visible = false;
                    gvFuelOil.MasterTableView.GetColumn("sulphur").Visible = false;
                }
                
                GridHeaderItem header = (GridHeaderItem)e.Item;
                if (ifueloil == 0) header["WTHeader"].Text = "Qty (Ltrs)";
                if (ifueloil == 1) header["WTHeader"].Text = "Qty (MT)"; 
            }

            DataRowView drv = (DataRowView)e.Item.DataItem;

            if (e.Item is GridEditableItem)
            {
                LinkButton att = (LinkButton)e.Item.FindControl("cmdAtt");
                if (att != null)
                {
                    att.Visible = SessionUtil.CanAccess(this.ViewState, att.CommandName);
                    if (drv["FLDISATTACHMENT"].ToString() == "0")
                    {
                        HtmlGenericControl html = new HtmlGenericControl();
                        html.InnerHtml = "<span class=\"icon\" style=\"color:gray\"><i class=\"fas fa-paperclip\"></i></span>";
                        att.Controls.Add(html);
                    }
                    att.Attributes.Add("onclick", "javascript:openNewWindow('att','','"+Session["sitepath"]+"/Common/CommonFileAttachment.aspx?dtkey=" + drv["FLDDTKEY"].ToString() + "&mod="
                        + PhoenixModule.VESSELACCOUNTS + "'); return false;");
                }
            }
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }
    protected void UcVessel_OnTextChangedEvent(object sender, EventArgs e)
    {
        try
        {
            Rebind();
        }
        catch (Exception ex)
        {
            ucError.HeaderMessage = "";
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }
    protected void Rebind()
    {
        gvFuelOil.SelectedIndexes.Clear();
        gvFuelOil.EditIndexes.Clear();
        gvFuelOil.DataSource = null;
        gvFuelOil.Rebind();
    }


    protected void gvFuelOil_NeedDataSource(object sender, Telerik.Web.UI.GridNeedDataSourceEventArgs e)
    {
        ViewState["PAGENUMBER"] = ViewState["PAGENUMBER"] != null ? ViewState["PAGENUMBER"] : gvFuelOil.CurrentPageIndex + 1;

        int iRowCount = 0;
        int iTotalPageCount = 0;

        string sortexpression = (ViewState["SORTEXPRESSION"] == null) ? null : (ViewState["SORTEXPRESSION"].ToString());
        int? sortdirection = null;
        if (ViewState["SORTDIRECTION"] != null)
            sortdirection = Int32.Parse(ViewState["SORTDIRECTION"].ToString());

        if (PhoenixSecurityContext.CurrentSecurityContext.VesselID == 0)
            ViewState["VESSELID"] = UcVessel.SelectedVessel == "" ? "0" : UcVessel.SelectedVessel;
        else
            ViewState["VESSELID"] = PhoenixSecurityContext.CurrentSecurityContext.VesselID;

        DataSet ds = PhoenixVesselPositionBunkerReceipt.BunkerReceiptSearch(
            General.GetNullableInteger(ViewState["VESSELID"].ToString()),
             General.GetNullableGuid(ViewState["OILTYPE"].ToString()),
             sortexpression, sortdirection,
             int.Parse(ViewState["PAGENUMBER"].ToString()),
             gvFuelOil.PageSize,
             ref iRowCount,
             ref iTotalPageCount);

        gvFuelOil.DataSource = ds;
        gvFuelOil.VirtualItemCount = iRowCount;

        if (ds.Tables[0].Rows.Count > 0)
        {
           
            ifueloil = int.Parse(ds.Tables[0].Rows[0]["FLDFUELOIL"].ToString());
        }


        ViewState["ROWCOUNT"] = iRowCount;
        ViewState["TOTALPAGECOUNT"] = iTotalPageCount;
    }

    protected void gvFuelOil_ItemCommand(object sender, GridCommandEventArgs e)
    {
        if(e.CommandName == "Page")
            {
            ViewState["PAGENUMBER"] = null;
        }
    }
}
