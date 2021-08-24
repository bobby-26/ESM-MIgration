using SouthNests.Phoenix.Framework;
using SouthNests.Phoenix.Registers;
using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Data;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Telerik.Web.UI;
public partial class Registers_RegisterProsperVesselMapping : PhoenixBasePage
{
    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {
            SessionUtil.PageAccessRights(this.ViewState);

            ViewState["COMPANYID"] = "";
            NameValueCollection nvc = PhoenixSecurityContext.CurrentSecurityContext.CompanyContext;
            if (nvc.Get("QMS") != null && nvc.Get("QMS") != "")
                ViewState["COMPANYID"] = nvc.Get("QMS");

            PhoenixToolbar toolbar = new PhoenixToolbar();
            toolbar.AddButton("Save", "SAVE",ToolBarDirection.Right);
            MenuTemplateMapping.AccessRights = this.ViewState;
            MenuTemplateMapping.MenuList = toolbar.Show();

            if (!IsPostBack)
            {
                ViewState["CATEGORYID"] = "";
                ViewState["MEASUREID"] = "";
                if (Request.QueryString["CATEGORYID"] != null && Request.QueryString["CATEGORYID"].ToString() != string.Empty)
                    ViewState["CATEGORYID"] = Request.QueryString["CATEGORYID"].ToString();
                if (Request.QueryString["MEASUREID"] != null && Request.QueryString["MEASUREID"].ToString() != string.Empty)
                    ViewState["MEASUREID"] = Request.QueryString["MEASUREID"].ToString();


                //DataTable dt = PhoenixRegisterProsperCategory.EditCategory(General.GetNullableGuid(ViewState["CATEGORYID"].ToString()));
                //if (dt.Rows.Count > 0)
                //{
                //    //txtevent.Text = dt.Rows[0]["FLDCATEGORYNAME"].ToString();
                //}
                DataTable  dt = PhoenixRegisterProsperMeasure.EditMeasure(General.GetNullableGuid(ViewState["MEASUREID"].ToString()));
                if (dt.Rows.Count > 0)
                {
                    txtmeasure.Text = dt.Rows[0]["FLDMEASURENAME"].ToString();
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
        DataSet ds = PhoenixRegisterProsperCategory.ProsperVesselTypeMappingList(General.GetNullableGuid(ViewState["CATEGORYID"].ToString()), General.GetNullableInteger(ViewState["COMPANYID"].ToString()), General.GetNullableGuid(ViewState["MEASUREID"].ToString()));
     

        cblvesseltype.DataSource = ds.Tables[0];
        cblvesseltype.DataTextField = "FLDTYPEDESCRIPTION";
        cblvesseltype.DataValueField = "FLDTYPE";
        cblvesseltype.DataBind();
    }
    protected void BindMapping()
    {
       // string companylist = null;
        BindSource();
        if (General.GetNullableGuid(ViewState["CATEGORYID"].ToString()) != null)
        {
            DataSet ds = PhoenixRegisterProsperCategory.GetProsperVesselTypeMappingList(
                General.GetNullableGuid(ViewState["CATEGORYID"].ToString())
                , General.GetNullableGuid(ViewState["MEASUREID"].ToString()));

          if (ds.Tables[0].Rows.Count > 0)
            {
                //cblsource.Enabled = false;
                for (int i = 0; i <= ds.Tables[0].Rows.Count - 1; i++)
                {
                    if (cblvesseltype.Items.FindByValue(ds.Tables[0].Rows[i]["FLDVESSELTYPEID"].ToString()) != null)
                        cblvesseltype.Items.FindByValue(ds.Tables[0].Rows[i]["FLDVESSELTYPEID"].ToString()).Selected = true;
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
                foreach (ListItem item in cblvesseltype.Items)
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
                    PhoenixRegisterProsperCategory.InsertProsperCategoryMapping(
                               General.GetNullableGuid(Request.QueryString["CATEGORYID"])
                               , General.GetNullableGuid(Request.QueryString["MEASUREID"])
                               , null
                               , strvesseltypeid.ToString());
                }




                ucStatus.Text = "Category mapped successfully.";
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
