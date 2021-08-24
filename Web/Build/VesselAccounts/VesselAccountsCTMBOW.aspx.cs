using System;
using System.Data;
using System.Web.UI.WebControls;
using SouthNests.Phoenix.Framework;
using SouthNests.Phoenix.VesselAccounts;
using Telerik.Web.UI;

public partial class VesselAccountsCTMBOW : PhoenixBasePage
{
    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {
            SessionUtil.PageAccessRights(this.ViewState);
            PhoenixToolbar toolbarmain = new PhoenixToolbar();
            toolbarmain.AddButton("General", "GENERAL");
            toolbarmain.AddButton("BOW", "BOW");
            toolbarmain.AddButton("CTM", "CTMCAL");
            toolbarmain.AddButton("Denomination", "DENOMINATOIN");
            toolbarmain.AddButton("List", "LIST");
            MenuCTMMain.AccessRights = this.ViewState;
            MenuCTMMain.MenuList = toolbarmain.Show();
            MenuCTMMain.SelectedMenuIndex = 1;
            if (!IsPostBack)
            {
                ViewState["CTMID"] = Request.QueryString["CTMID"];
                ViewState["ACTIVEYN"] = Request.QueryString["a"];
                gvCTM.PageSize = int.Parse(PhoenixGeneralSettings.CurrentGeneralSetting.Records);
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
        try
        {
            DataTable dt = PhoenixVesselAccountsCTM.ListCaptainCashBOW(new Guid(ViewState["CTMID"].ToString()));
            gvCTM.DataSource = dt;
            gvCTM.VirtualItemCount = dt.Rows.Count;
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }
    protected void gvCTM_NeedDataSource(object sender, GridNeedDataSourceEventArgs e)
    {
        try
        {
            BindData();
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }
    protected void Rebind()
    {
        gvCTM.SelectedIndexes.Clear();
        gvCTM.EditIndexes.Clear();
        gvCTM.DataSource = null;
        gvCTM.Rebind();
    }
    protected void gvCTM_ItemCommand(object sender, GridCommandEventArgs e)
    {
        try
        {           
            if (e.CommandName.ToUpper().Equals("SAVE"))
            {                
                Guid? id = General.GetNullableGuid(((RadLabel)e.Item.FindControl("lblbowid")).Text.Trim());
                string signondate = ((RadLabel)e.Item.FindControl("lblsignondate")).Text;
                string employeeid = ((RadLabel)e.Item.FindControl("lblEmployeeId")).Text;
                string Signonoffid = ((RadLabel)e.Item.FindControl("lblSignonoffid")).Text;
                string date = ((UserControlDate)e.Item.FindControl("txtDate")).Text;
                if (!IsValidBOW(date, signondate))
                {
                    ucError.Visible = true;
                    return;
                }
                PhoenixVesselAccountsCTM.InsertCaptainCashBOW(new Guid(ViewState["CTMID"].ToString()), int.Parse(employeeid), DateTime.Parse(date), id, General.GetNullableInteger(Signonoffid));
                Rebind();
            }
            if (e.CommandName.ToUpper().Equals("DELETE"))
            {
                Guid id = new Guid(((RadLabel)e.Item.FindControl("lblbowid")).Text.Trim());
                PhoenixVesselAccountsCTM.DeleteCaptainCashBOW(id);
                Rebind();
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
    decimal r;
    protected void gvCTM_ItemDataBound(Object sender, GridItemEventArgs e)
    {
        DataRowView drv = (DataRowView)e.Item.DataItem;
        if (e.Item is GridHeaderItem)
            r = 0;
        if (e.Item is GridEditableItem)
        {
             
            LinkButton ed = (LinkButton)e.Item.FindControl("cmdEdit");
            if (ed != null) ed.Visible = SessionUtil.CanAccess(this.ViewState, ed.CommandName);
            LinkButton db = (LinkButton)e.Item.FindControl("cmdDelete");
            if (db != null)
            {
                db.Visible = SessionUtil.CanAccess(this.ViewState, db.CommandName);
                db.Attributes.Add("onclick", "return fnConfirmDelete(event); return false;");
                if (drv["FLDBOWID"].ToString() == string.Empty) db.Visible = false;
            }
            r = r + decimal.Parse((drv["FLDAMOUNT"].ToString() != string.Empty ? drv["FLDAMOUNT"].ToString() : "0"));
            if (ViewState["ACTIVEYN"].ToString() != "1")
            {
                if (db != null) db.Visible = false;
                if (ed != null) ed.Visible = false;
            }

        }
        if (e.Item is GridFooterItem)
        {
            e.Item.Cells[5].Text = r.ToString();
        }
    }
  
    protected void MenuCTMMain_TabStripCommand(object sender, EventArgs e)
    {
        try
        {
            RadToolBarEventArgs dce = (RadToolBarEventArgs)e;
            string CommandName = ((RadToolBarButton)dce.Item).CommandName;
            if (CommandName.ToUpper().Equals("GENERAL") || ViewState["CTMID"] == null)
            {
                ViewState["SETCURRENTNAVIGATIONTAB"] = "../VesselAccounts/VesselAccountsCTMGeneral.aspx";
            }
            else if (CommandName.ToUpper().Equals("BOW"))
            {
                ViewState["SETCURRENTNAVIGATIONTAB"] = "../VesselAccounts/VesselAccountsCTMBOW.aspx";
            }
            else if (CommandName.ToUpper().Equals("DENOMINATOIN"))
            {
                ViewState["SETCURRENTNAVIGATIONTAB"] = "../VesselAccounts/VesselAccountsCTMDenomination.aspx";
            }
            else if (CommandName.ToUpper().Equals("CTMCAL"))
            {
                ViewState["SETCURRENTNAVIGATIONTAB"] = "../VesselAccounts/VesselAccountsCTMCalculation.aspx";
            }
            if (CommandName.ToUpper().Equals("LIST"))
            {
                Response.Redirect("../VesselAccounts/VesselAccountsCTM.aspx", false);
            }
            else
                Response.Redirect(ViewState["SETCURRENTNAVIGATIONTAB"].ToString() + "?CTMID=" + ViewState["CTMID"] + "&a=" + ViewState["ACTIVEYN"], false);
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }
 
    private bool IsValidBOW(string date, string signon)
    {
        ucError.HeaderMessage = "Please provide the following required information";
        DateTime resultDate;
        if (!General.GetNullableDateTime(date).HasValue)
        {
            ucError.ErrorMessage = "Relief Due Date is required.";
        }
        else if (DateTime.TryParse(date, out resultDate) && DateTime.Compare(resultDate, DateTime.Parse(signon)) < 0)
        {
            ucError.ErrorMessage = "Relief Due Date should be later than sign on date.";
        }
        return (!ucError.IsError);
    }

}
