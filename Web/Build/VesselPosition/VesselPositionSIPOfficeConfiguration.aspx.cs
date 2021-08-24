using System;
using System.Data;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using SouthNests.Phoenix.Framework;
using SouthNests.Phoenix.Registers;
using SouthNests.Phoenix.Common;
using SouthNests.Phoenix.VesselPosition;
using System.Web;
using System.IO;
using System.Net;
using SouthNests.Phoenix.Integration;
using Telerik.Web.UI;

public partial class VesselPositionSIPOfficeConfiguration : PhoenixBasePage
{
    protected void Page_Load(object sender, EventArgs e)
    {
        SessionUtil.PageAccessRights(this.ViewState);

        PhoenixToolbar toolbabutton = new PhoenixToolbar();
        toolbabutton.AddButton("Save", "SAVE",ToolBarDirection.Right);
        MenuProcedureDetailList.AccessRights = this.ViewState;
        MenuProcedureDetailList.MenuList = toolbabutton.Show();

        if (!IsPostBack)
        {        
            BindData();
        }
    }
 
    private void BindData()
    {
        DataSet ds = PhoenixVesselPositionSIPConfiguration.EditSIPfficeinstruction();

        if (ds.Tables[0].Rows.Count > 0)
        {
            DataRow dr = ds.Tables[0].Rows[0];

            txtriskassesment.Text = dr["FLDRISKASSESSMENT"].ToString();

            txtstructuralmodifiation.Text = dr["FLDSTRUCTURALMODIFY"].ToString();
            txtscrubber.Text = dr["FLDSCRUBBER"].ToString();

            txttankclean.Text = dr["FLDTANKCLEANCOMMENT"].ToString();
            txtbunkercomment.Text = dr["FLDBUNKERCOMMENT"].ToString();

            txtpurchaseprocedure.Text = dr["FLDFUELPURCHASE"].ToString();
            txtalternate.Text = dr["FLDALTERNATESTEP"].ToString();
            txtcomplientfuel.Text = dr["FLDCOMPLIEDTFUEL"].ToString();
            txtnoncomplient.Text = dr["FLDNONCOMPLIENTFUEL"].ToString();
            txtfuelhcange.Text = dr["FLDFUELCHANGEOVER"].ToString();
            txttrainingdetails.Text = dr["FLDTRAININGDETAIL"].ToString();
            txtDocumentation.Text = dr["FLDDOCNONCOMPLIENT"].ToString();
            txtnonavailability.Text = dr["FLDDOCNONAVAILABLITY"].ToString();

        }

    }
    protected void MenuProcedureDetailList_TabStripCommand(object sender, EventArgs e)
    {
        try
        {
            RadToolBarEventArgs dce = (RadToolBarEventArgs)e;
            if (((RadToolBarButton)dce.Item).CommandName.ToUpper().Equals("SAVE"))
            {

                PhoenixVesselPositionSIPConfiguration.InsertSIPOfficeInstruction(
                    General.GetNullableString(txtriskassesment.Text)
                    , General.GetNullableString(txtpurchaseprocedure.Text)
                    , General.GetNullableString(txtalternate.Text)
                    , General.GetNullableString(txtcomplientfuel.Text)
                    , General.GetNullableString(txtnoncomplient.Text)
                    , General.GetNullableString(txtfuelhcange.Text)
                    , General.GetNullableString(txttrainingdetails.Text)
                    , General.GetNullableString(txtDocumentation.Text)
                    , General.GetNullableString(txtnonavailability.Text)
                    , General.GetNullableString(txttankclean.Text)
                    , General.GetNullableString(txtbunkercomment.Text)
                    , General.GetNullableString(txtstructuralmodifiation.Text)
                    , General.GetNullableString(txtscrubber.Text)
                            );

                BindData();
            }

                ucStatus.Text = "Information saved successfully.";

        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }
 
}