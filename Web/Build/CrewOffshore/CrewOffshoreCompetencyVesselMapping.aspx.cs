using SouthNests.Phoenix.CrewOffshore;
using SouthNests.Phoenix.Framework;
using SouthNests.Phoenix.Registers;
using System;
using System.Data;
using System.Text;
using Telerik.Web.UI;

public partial class CrewOffshore_CrewOffshoreCompetencyVesselMapping : PhoenixBasePage
{
    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {
            SessionUtil.PageAccessRights(this.ViewState);
            PhoenixToolbar toolbar = new PhoenixToolbar();
            toolbar.AddButton("Save", "SAVE", ToolBarDirection.Right);
            MenuTemplateMapping.AccessRights = this.ViewState;
            MenuTemplateMapping.MenuList = toolbar.Show();

            if (!IsPostBack)
            {
                ViewState["CATEGORYID"] = "";
                ViewState["SUBCATEGORYID"] = "";
                ViewState["ID"] = "";
                if (Request.QueryString["CID"] != null && Request.QueryString["CID"].ToString() != string.Empty)
                    ViewState["CID"] = Request.QueryString["CID"].ToString();

                txtevent.Text = ViewState["CID"].ToString();

                DataTable dt = PhoenixCrewOffshoreTrainingSubCategory.EditTrainingSubCategory(General.GetNullableInteger(ViewState["CID"].ToString()));
                if (dt.Rows.Count > 0)
                {
                    txtevent.Text = dt.Rows[0]["FLDCATEGORYNAME"].ToString();
                    txtmeasure.Text = dt.Rows[0]["FLDSUBCATEGORYNAME"].ToString();
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
        DataSet ds = PhoenixRegistersVesselType.ListVesselType(1);
        cblvesseltype.DataSource = ds.Tables[0];
        cblvesseltype.DataBindings.DataTextField = "FLDTYPEDESCRIPTION";
        cblvesseltype.DataBindings.DataValueField = "FLDVESSELTYPEID";
        cblvesseltype.DataBind();
    }
    protected void BindMapping()
    {
        // string companylist = null;
        BindSource();
        if (General.GetNullableInteger(ViewState["CID"].ToString()) != null)
        {
            DataTable dt = PhoenixCrewOffshoreTrainingSubCategory.GetSubCategoryVesseltype(
               General.GetNullableInteger(ViewState["CID"].ToString()));

            if (dt.Rows.Count > 0)
            {
                //cblsource.Enabled = false;
                for (int i = 0; i <= dt.Rows.Count - 1; i++)
                {
                    cblvesseltype.SelectedValue = dt.Rows[i]["FLDVESSELTYPE"].ToString();
                    // if (cblvesseltype.Items.FindByValue(ds.Tables[0].Rows[i]["FLDVESSELTYPE"].ToString()) != null)
                    // cblvesseltype.Items.FindByValue(ds.Tables[0].Rows[i]["FLDVESSELTYPE"].ToString()).Selected = true;
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
                    PhoenixCrewOffshoreTrainingSubCategory.UpdateSubCategoryVesselType(
                               General.GetNullableInteger(Request.QueryString["CID"])
                              , strvesseltypeid.ToString());
                }
                else
                {
                    PhoenixCrewOffshoreTrainingSubCategory.UpdateSubCategoryVesselType(
                        General.GetNullableInteger(Request.QueryString["CID"])
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