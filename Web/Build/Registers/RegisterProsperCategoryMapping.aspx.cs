using System;
using System.Data;
using System.Web.UI.WebControls;
using SouthNests.Phoenix.Framework;
using SouthNests.Phoenix.Registers;
using System.Text;
using System.Collections.Specialized;
using Telerik.Web.UI;



public partial class Registers_RegisterProsperCategoryMapping : PhoenixBasePage
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
              


                DataTable dt = PhoenixRegisterProsperCategory.EditCategory(General.GetNullableGuid(ViewState["CATEGORYID"].ToString()));
                if(dt.Rows.Count>0)
                {
                    txtevent.Text = dt.Rows[0]["FLDCATEGORYNAME"].ToString();
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
        DataSet ds = PhoenixRegisterProsperCategory.ProsperCategoryMappingList(General.GetNullableGuid(ViewState["CATEGORYID"].ToString()),General.GetNullableInteger(ViewState["COMPANYID"].ToString()));
        cblextaudit.DataSource = ds.Tables[0];
        cblextaudit.DataTextField = "FLDSHORTCODE";
        cblextaudit.DataValueField = "FLDINSPECTIONID";
        cblextaudit.DataBind();

        cblintaudit.DataSource = ds.Tables[1];
        cblintaudit.DataTextField = "FLDSHORTCODE";
        cblintaudit.DataValueField = "FLDINSPECTIONID";
        cblintaudit.DataBind();

        cblextvetting.DataSource = ds.Tables[2];
        cblextvetting.DataTextField = "FLDSHORTCODE";
        cblextvetting.DataValueField = "FLDINSPECTIONID";
        cblextvetting.DataBind();

        cblintvetting.DataSource = ds.Tables[3];
        cblintvetting.DataTextField = "FLDSHORTCODE";
        cblintvetting.DataValueField = "FLDINSPECTIONID";
        cblintvetting.DataBind();

        cblincident.DataSource = ds.Tables[4];
        cblincident.DataTextField = "FLDNAME";
        cblincident.DataValueField = "FLDINCIDENTNEARMISSCATEGORYID";
        cblincident.DataBind();

        cblfeedback.DataSource = ds.Tables[5];
        cblfeedback.DataTextField = "FLDFEEDBACKCATEGORYNAME";
        cblfeedback.DataValueField = "FLDFEEDBACKCATEGORYID";
        cblfeedback.DataBind();


        //cblvesseltype.DataSource = ds.Tables[6];
        //cblvesseltype.DataTextField = "FLDTYPEDESCRIPTION";
        //cblvesseltype.DataValueField = "FLDTYPE";
        //cblvesseltype.DataBind();
    }

    protected void BindMapping()
    {
       // string companylist = null;
        BindSource();
        if (General.GetNullableGuid(ViewState["CATEGORYID"].ToString()) != null)
        {
            DataSet ds= PhoenixRegisterProsperCategory.GetProsperCategoryMappingList(General.GetNullableGuid(ViewState["CATEGORYID"].ToString()));

            if(ds.Tables[0].Rows.Count>0)
            {
               // cblvesseltype.Enabled = false;
                for(int i= 0;i<= ds.Tables[0].Rows.Count-1;i++)
                {
                    if (cblextaudit.Items.FindByValue(ds.Tables[0].Rows[i]["FLDINSPECTIONID"].ToString().ToLower()) != null)
                        cblextaudit.Items.FindByValue(ds.Tables[0].Rows[i]["FLDINSPECTIONID"].ToString().ToLower()).Selected = true;

                    if (cblintaudit.Items.FindByValue(ds.Tables[0].Rows[i]["FLDINSPECTIONID"].ToString().ToLower()) != null)
                        cblintaudit.Items.FindByValue(ds.Tables[0].Rows[i]["FLDINSPECTIONID"].ToString().ToLower()).Selected = true;

                    if (cblextvetting.Items.FindByValue(ds.Tables[0].Rows[i]["FLDINSPECTIONID"].ToString().ToLower()) != null)
                        cblextvetting.Items.FindByValue(ds.Tables[0].Rows[i]["FLDINSPECTIONID"].ToString().ToLower()).Selected = true;

                    if (cblintvetting.Items.FindByValue(ds.Tables[0].Rows[i]["FLDINSPECTIONID"].ToString().ToLower()) != null)
                        cblintvetting.Items.FindByValue(ds.Tables[0].Rows[i]["FLDINSPECTIONID"].ToString().ToLower()).Selected = true;

                    if (cblincident.Items.FindByValue(ds.Tables[0].Rows[i]["FLDINSPECTIONID"].ToString().ToLower()) != null)
                        cblincident.Items.FindByValue(ds.Tables[0].Rows[i]["FLDINSPECTIONID"].ToString().ToLower()).Selected = true;

                    if (cblfeedback.Items.FindByValue(ds.Tables[0].Rows[i]["FLDINSPECTIONID"].ToString().ToLower()) != null)
                        cblfeedback.Items.FindByValue(ds.Tables[0].Rows[i]["FLDINSPECTIONID"].ToString().ToLower()).Selected = true;
                }
               
            }

            //if (ds.Tables[1].Rows.Count > 0)
            //{
            //    //cblsource.Enabled = false;
            //    for (int i = 0; i <= ds.Tables[1].Rows.Count - 1; i++)
            //    {
            //        if (cblvesseltype.Items.FindByValue(ds.Tables[1].Rows[i]["FLDVESSELTYPEID"].ToString()) != null)
            //            cblvesseltype.Items.FindByValue(ds.Tables[1].Rows[i]["FLDVESSELTYPEID"].ToString()).Selected = true;
            //    }
            //}
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
                foreach (ListItem item in cblextaudit.Items)
                {
                    if (item.Selected == true && item.Enabled == true)
                    {
                        strcategoryid.Append(item.Value.ToString());
                        strcategoryid.Append(",");
                    }
                }
                foreach (ListItem item in cblintaudit.Items)
                {
                    if (item.Selected == true && item.Enabled == true)
                    {
                        strcategoryid.Append(item.Value.ToString());
                        strcategoryid.Append(",");
                    }
                }
                foreach (ListItem item in cblextvetting.Items)
                {
                    if (item.Selected == true && item.Enabled == true)
                    {
                        strcategoryid.Append(item.Value.ToString());
                        strcategoryid.Append(",");
                    }
                }

                foreach (ListItem item in cblintvetting.Items)
                {
                    if (item.Selected == true && item.Enabled == true)
                    {
                        strcategoryid.Append(item.Value.ToString());
                        strcategoryid.Append(",");
                    }
                }

                foreach (ListItem item in cblincident.Items)
                {
                    if (item.Selected == true && item.Enabled == true)
                    {
                        strcategoryid.Append(item.Value.ToString());
                        strcategoryid.Append(",");
                    }
                }

                foreach (ListItem item in cblfeedback.Items)
                {
                    if (item.Selected == true && item.Enabled == true)
                    {
                        strcategoryid.Append(item.Value.ToString());
                        strcategoryid.Append(",");
                    }
                }

                if (strcategoryid.Length > 1)
                {
                    strcategoryid.Remove(strcategoryid.Length - 1, 1);
                    PhoenixRegisterProsperCategory.InsertProsperCategoryMapping(
                               General.GetNullableGuid(Request.QueryString["CATEGORYID"])
                               , strcategoryid.ToString()
                               ,null);
                }

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

    private bool IsValidMapping(string companyid)
    {
        ucError.HeaderMessage = "Please provide the following required information";

        if (General.GetNullableString(companyid) == null)
            ucError.ErrorMessage = "Company is required.";

        return (!ucError.IsError);
    }
}
