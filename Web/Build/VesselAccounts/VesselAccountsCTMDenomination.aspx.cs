using System;
using System.Collections.Specialized;
using System.Data;
using System.Web.UI;
using System.Web.UI.WebControls;
using SouthNests.Phoenix.Framework;
using SouthNests.Phoenix.VesselAccounts;
using Telerik.Web.UI;

public partial class VesselAccountsCTMDenomination : PhoenixBasePage
{
    private decimal d = 0;
    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {
            PhoenixToolbar toolbarmain = new PhoenixToolbar();
            toolbarmain.AddButton("General", "GENERAL");
            toolbarmain.AddButton("BOW", "BOW");
            toolbarmain.AddButton("CTM", "CTMCAL");
            toolbarmain.AddButton("Denomination", "DENOMINATOIN");
            toolbarmain.AddButton("List", "LIST");
            MenuCTMMain.AccessRights = this.ViewState;
            MenuCTMMain.MenuList = toolbarmain.Show();
            MenuCTMMain.SelectedMenuIndex = 3;
            if (!IsPostBack)
            {

                ViewState["CTMID"] = Request.QueryString["CTMID"];
                ViewState["ACTIVEYN"] = Request.QueryString["a"];
                if (ViewState["CTMID"] != null)
                    EditCTM(new Guid(ViewState["CTMID"].ToString()));
            }
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }
    private void EditCTM(Guid CTMId)
    {
        DataTable dt = PhoenixVesselAccountsCTM.EditCTMRequest(CTMId);
        if (dt.Rows.Count > 0)
        {
            DataRow dr = dt.Rows[0];
            txtAmount.Text = dr["FLDAMOUNT"].ToString();
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
    public void BindData()
    {
        try
        {
            DataTable dt = PhoenixVesselAccountsCTM.ListDenomination(new Guid(ViewState["CTMID"].ToString()), ref d);
            gvDenomination.DataSource = dt;

        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }
    protected void gvDenomination_NeedDataSource(object sender, GridNeedDataSourceEventArgs e)
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
        gvDenomination.SelectedIndexes.Clear();
        gvDenomination.EditIndexes.Clear();
        gvDenomination.DataSource = null;
        gvDenomination.Rebind();
    }
    protected void gvDenomination_ItemCommand(object sender, GridCommandEventArgs e)
    {
        try
        {
            if (e.CommandName.ToUpper().Equals("SAVE"))
            {
                Guid? id = General.GetNullableGuid(((RadLabel)e.Item.FindControl("lbldenominationId")).Text.Trim());
                string denomination = ((Label)e.Item.FindControl("lblDenomination")).Text;
                string notes = ((UserControlMaskNumber)e.Item.FindControl("txtNotes")).Text;
                if (!IsValidDenomination(notes))
                {
                    ucError.Visible = true;
                    return;
                }
                if (!id.HasValue)
                    PhoenixVesselAccountsCTM.InsertDenomination(PhoenixSecurityContext.CurrentSecurityContext.VesselID, new Guid(ViewState["CTMID"].ToString())
                                                        , int.Parse(denomination), int.Parse(notes));
                else
                    PhoenixVesselAccountsCTM.UpdateDenomination(id.Value, int.Parse(notes));

                Rebind();
            }
            if (e.CommandName.ToUpper().Equals("DELETE"))
            {
                Guid? id = General.GetNullableGuid(((RadLabel)e.Item.FindControl("lbldenominationId")).Text.Trim());
                PhoenixVesselAccountsCTM.DeleteDenomination(id.Value);
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

    protected void gvDenomination_ItemDataBound(Object sender, GridItemEventArgs e)
    {
        try
        {
            DataRowView drv = (DataRowView)e.Item.DataItem;

            if (e.Item is GridEditableItem)
            {

                LinkButton ed = (LinkButton)e.Item.FindControl("cmdEdit");
                if (ed != null) ed.Visible = SessionUtil.CanAccess(this.ViewState, ed.CommandName);
                LinkButton db = (LinkButton)e.Item.FindControl("cmdDelete");
                if (db != null) db.Visible = SessionUtil.CanAccess(this.ViewState, db.CommandName);
                if (db != null) db.Attributes.Add("onclick", "return fnConfirmDelete(event); return false;");

                if (db != null && drv["FLDDENOMINATIONID"].ToString() == string.Empty) db.Visible = false;
                if (ViewState["ACTIVEYN"].ToString() != "1")
                {
                    if (db != null) db.Visible = false;
                    if (ed != null) ed.Visible = false;
                }

            }
            if (e.Item is GridFooterItem)
            {
                RadLabel lblTotalamount = (RadLabel)e.Item.FindControl("lblTotalamount");
                lblTotalamount.Text = d.ToString();
            }
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }

    private bool IsValidDenomination(string notes)
    {
        ucError.HeaderMessage = "Please provide the following required information";
        if (!General.GetNullableInteger(notes).HasValue)
        {
            ucError.ErrorMessage = "Notes is required.";
        }
        return (!ucError.IsError);
    }

}