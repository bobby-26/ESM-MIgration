using SouthNests.Phoenix.Framework;
using SouthNests.Phoenix.Registers;
using System;
using System.Data;
using System.Text;
using System.Web.UI.WebControls;
using Telerik.Web.UI;

public partial class Registers_RegisterPersonalProfileVesselType : PhoenixBasePage
{
    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {
            SessionUtil.PageAccessRights(this.ViewState);

            PhoenixToolbar toolbar = new PhoenixToolbar();
            toolbar.AddButton("Save", "SAVE",ToolBarDirection.Right);
            MenuTemplateMapping.AccessRights = this.ViewState;
            MenuTemplateMapping.MenuList = toolbar.Show();

            if (!IsPostBack)
            {
                ViewState["CATEGORYID"] = "";
                ViewState["EVALUATION"] = "";
                ViewState["ID"] = "";
                if (Request.QueryString["ID"] != null && Request.QueryString["ID"].ToString() != string.Empty)
                    ViewState["ID"] = Request.QueryString["ID"].ToString();                
                DataTable dt = PhoenixRegisterPersonalProfileVesselType.EditPersonalProfile(General.GetNullableInteger(ViewState["ID"].ToString()));
                if (dt.Rows.Count > 0)
                {
                    txtevent.Text = dt.Rows[0]["FLDPROFILEQUESTION"].ToString(); 
                    txtmeasure.Text = dt.Rows[0]["FLDPROFILECATEGORY"].ToString();
                }
                BindSource();
                BindMapping();
            }
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }
    protected void BindSource()
    {
        DataSet ds = PhoenixRegisterPersonalProfileVesselType.PersonalProfileVesselTypeMappingList();
        cblvesseltype.DataSource = ds.Tables[0];
        cblvesseltype.DataBindings.DataTextField = "FLDTYPEDESCRIPTION";
        cblvesseltype.DataBindings.DataValueField = "FLDTYPE";
        cblvesseltype.DataBind();
    }

    protected void BindMapping()
    {
        // string companylist = null;
        BindSource();
        if (General.GetNullableInteger(ViewState["ID"].ToString()) != null)
        {
            DataSet ds = PhoenixRegisterPersonalProfileVesselType.GetPersonalProfileVesselTypeMappingList(
               General.GetNullableInteger(ViewState["ID"].ToString()));

            if (ds.Tables[0].Rows.Count > 0)
            {
                //cblsource.Enabled = false;
                for (int i = 0; i <= ds.Tables[0].Rows.Count - 1; i++)
                {
                    cblvesseltype.SelectedValue = ds.Tables[0].Rows[i]["FLDVESSELTYPE"].ToString();
                    //if (cblvesseltype.Items.FindByValue(ds.Tables[0].Rows[i]["FLDVESSELTYPE"].ToString()) != null)
                    //    cblvesseltype.Items.FindByValue(ds.Tables[0].Rows[i]["FLDVESSELTYPE"].ToString()).Selected = true;
                }
            }
        }
    }

    protected void MenuTemplateMapping_TabStripCommand(object sender, EventArgs e)
    {
        try
        {
            RadToolBarEventArgs dce = (RadToolBarEventArgs)e;
            string CommandName = ((RadToolBarButton)dce.Item).CommandName;
            if (CommandName.ToUpper().Equals("SAVE"))
            {
                StringBuilder strcategoryid = new StringBuilder();
                StringBuilder strvesseltypeid = new StringBuilder();
                foreach (ButtonListItem item in cblvesseltype.Items)
                {
                    if (item.Selected == true && item.Enabled == true)
                    {
                        strvesseltypeid.Append(item.Value.ToString());
                        strvesseltypeid.Append(",");
                    }
                }

                if (strvesseltypeid.Length > 1)
                {
                    strvesseltypeid.Remove(strvesseltypeid.Length - 1, 1);
                    PhoenixRegisterPersonalProfileVesselType.InsertPersonalProfileVesselTypeMapping(
                               General.GetNullableInteger(Request.QueryString["ID"])
                              , strvesseltypeid.ToString());
                }
                else
                {
                    PhoenixRegisterPersonalProfileVesselType.InsertPersonalProfileVesselTypeMapping(
                               General.GetNullableInteger(Request.QueryString["ID"])
                              , null);
                }
                ucStatus.Text = "Vessel type mapped successfully.";
                //BindData();
            }
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }

}
