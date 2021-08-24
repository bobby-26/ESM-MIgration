using System;
using System.Data;
using System.Web.UI;
using System.Web.UI.WebControls;
using SouthNests.Phoenix.Framework;
using SouthNests.Phoenix.Registers;
using Telerik.Web.UI;
public partial class RegistersVPRSSOX : PhoenixBasePage
{
    protected void Page_Load(object sender, EventArgs e)
    {
        SessionUtil.PageAccessRights(this.ViewState);
        if (!IsPostBack)
        {
            
        }
    }

    private void BindSOX()
    {
        try
        {
            DataSet ds = PhoenixRegistersSOX.ListVPRSSOX();
            gvSOX.DataSource = ds;
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }
    protected void Rebind()
    {
        gvSOX.SelectedIndexes.Clear();
        gvSOX.EditIndexes.Clear();
        gvSOX.DataSource = null;
        gvSOX.Rebind();
    }
    
    protected void gvSOX_ItemDataBound(Object sender, GridItemEventArgs e)
    {
        DataRowView drv = (DataRowView)e.Item.DataItem;

        ImageButton db = (ImageButton)e.Item.FindControl("cmdDelete");
        if (db != null) db.Attributes.Add("onclick", "return fnConfirmDelete(event); return false;");
        
        if (e.Item is GridEditableItem)
        {
            if (drv["FLDTIER"].ToString() == "")
            {
                if (drv["FLDBSOUT"].ToString() != "")
                {
                    Label lblbsout = (Label)e.Item.FindControl("lblbsout");
                    if(lblbsout != null)
                        lblbsout.Text = Convert.ToInt32(drv["FLDBSOUT"]).ToString();

                    UserControlMaskNumber ucbsoutEdit = (UserControlMaskNumber)e.Item.FindControl("ucbsoutEdit");
                    if (ucbsoutEdit != null)
                    {
                        ucbsoutEdit.Text = Convert.ToInt32(drv["FLDBSOUT"]).ToString();
                        ucbsoutEdit.IsInteger = true;
                    }

                    Label lblbsin = (Label)e.Item.FindControl("lblbsin");
                    if (lblbsin != null)
                        lblbsin.Text = Convert.ToInt32(drv["FLDBSIN"]).ToString();

                    UserControlMaskNumber ucbsinEdit = (UserControlMaskNumber)e.Item.FindControl("ucbsinEdit");
                    if (ucbsinEdit != null)
                    {
                        ucbsinEdit.Text = Convert.ToInt32(drv["FLDBSIN"]).ToString();
                        ucbsinEdit.IsInteger = true;
                    }

                    Label lblbsberth = (Label)e.Item.FindControl("lblbsberth");
                    if (lblbsberth != null)
                        lblbsberth.Text = Convert.ToInt32(drv["FLDBSBERTH"]).ToString();

                    UserControlMaskNumber ucbsberthEdit = (UserControlMaskNumber)e.Item.FindControl("ucbsberthEdit");
                    if (ucbsberthEdit != null)
                    {
                        ucbsberthEdit.Text = Convert.ToInt32(drv["FLDBSBERTH"]).ToString();
                        ucbsberthEdit.IsInteger = true;
                    }
                }
            }
        }
    }
    protected void gvSOX_ItemCommand(object sender, GridCommandEventArgs e)
    {
        try
        {
            if (e.CommandName.ToUpper().Equals("DELETE"))
            {
                BindSOX();
            }
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }
    private bool IsValidOperationalTimeSummary(string MeteorologyValue)
    {
        ucError.HeaderMessage = "Please provide the following required information";

        if (MeteorologyValue.Trim().Equals(""))
            ucError.ErrorMessage = "Value is required.";

        return (!ucError.IsError);
    }

    protected void gvSOX_UpdateCommand(object sender, GridCommandEventArgs e)
    {
        try
        {
            string bsout = "";
            string bsin = "";
            string bsberth = "";
            string outweightage = "";
            string inweightage = "";
            string berthweightage = "";

            string tier = ((Label)e.Item.FindControl("lbltieridEdit")).Text;

            if (tier != "")
            {
                bsout = ((UserControlMaskNumber)e.Item.FindControl("ucbsoutEdit")).Text;
                bsin = ((UserControlMaskNumber)e.Item.FindControl("ucbsinEdit")).Text;
                bsberth = ((UserControlMaskNumber)e.Item.FindControl("ucbsberthEdit")).Text;
            }
            else
            {
                tier = "0";
                outweightage = ((UserControlMaskNumber)e.Item.FindControl("ucbsoutEdit")).Text;
                inweightage = ((UserControlMaskNumber)e.Item.FindControl("ucbsinEdit")).Text;
                berthweightage = ((UserControlMaskNumber)e.Item.FindControl("ucbsberthEdit")).Text;
            }


            PhoenixRegistersSOX.InsertVPRSSOX(PhoenixSecurityContext.CurrentSecurityContext.UserCode, int.Parse(tier)
                , General.GetNullableDecimal(bsout)
                , General.GetNullableDecimal(bsin)
                , General.GetNullableDecimal(bsberth)
                , General.GetNullableDecimal(outweightage)
                , General.GetNullableDecimal(inweightage)
                , General.GetNullableDecimal(berthweightage));

            BindSOX();
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }

    protected void gvSOX_NeedDataSource(object sender, GridNeedDataSourceEventArgs e)
    {
        try
        {
            BindSOX();
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }
}
