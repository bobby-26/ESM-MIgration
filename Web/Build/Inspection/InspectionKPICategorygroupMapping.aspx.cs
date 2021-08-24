using System;
using System.Collections;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Xml.Linq;
using SouthNests.Phoenix.Framework;
using SouthNests.Phoenix.Common;
using SouthNests.Phoenix.Registers;
using SouthNests.Phoenix.Inspection;
using System.Text;
using System.Collections.Specialized;


public partial class Inspection_InspectionKPICategorygroupMapping : PhoenixBasePage
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

            if (!IsPostBack)
            {
                ViewState["CATEGORYGROUPID"] = "";
                
                ViewState["CATEGORYCODE"] = "";
                if (Request.QueryString["CATEGORYGROUPID"] != null && Request.QueryString["CATEGORYGROUPID"].ToString() != string.Empty)
                    ViewState["CATEGORYGROUPID"] = Request.QueryString["CATEGORYGROUPID"].ToString();

                if (Request.QueryString["CATEGORYCODE"] != null && Request.QueryString["CATEGORYCODE"].ToString() != string.Empty)
                    ViewState["CATEGORYCODE"] = Request.QueryString["CATEGORYCODE"].ToString();


                DataTable dt = PhoenixKPIRegisters.kpiscoringcriteriagroupsearch(General.GetNullableInteger(ViewState["CATEGORYCODE"].ToString())
                    ,General.GetNullableGuid(ViewState["CATEGORYGROUPID"].ToString()));
                if (dt.Rows.Count > 0)
                {
                    txtevent.Text = dt.Rows[0]["FLDCATEGORYSHORTCODE"].ToString();
                }
               
                BindSource();
                BindMapping();

            }
            PhoenixToolbar toolbar = new PhoenixToolbar();
            toolbar.AddButton("Save", "SAVE");
            MenuTemplateMapping.AccessRights = this.ViewState;
            MenuTemplateMapping.MenuList = toolbar.Show();
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }
    protected void BindSource()
    {
        DataSet ds = PhoenixKPIRegisters.kpigroupmappingsearch(General.GetNullableInteger(ViewState["COMPANYID"].ToString()));
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

      
        //cblvesseltype.DataSource = ds.Tables[6];
        //cblvesseltype.DataTextField = "FLDTYPEDESCRIPTION";
        //cblvesseltype.DataValueField = "FLDTYPE";
        //cblvesseltype.DataBind();
    }

    protected void BindMapping()
    {
        // string companylist = null;
        BindSource();
        if (General.GetNullableGuid(ViewState["CATEGORYGROUPID"].ToString()) != null)
        {
            DataTable dt = PhoenixKPIRegisters.kpigroupmappingsearch(new Guid(ViewState["CATEGORYGROUPID"].ToString()));

            if (dt.Rows.Count > 0)
            {
                //    // cblvesseltype.Enabled = false;
                for (int i = 0; i <= dt.Rows.Count - 1; i++)
                {
                    if (cblextaudit.Items.FindByValue(dt.Rows[i]["FLDITEM"].ToString().ToLower()) != null)
                        cblextaudit.Items.FindByValue(dt.Rows[i]["FLDITEM"].ToString().ToLower()).Selected = true;

                    if (cblintaudit.Items.FindByValue(dt.Rows[i]["FLDITEM"].ToString().ToLower()) != null)
                        cblintaudit.Items.FindByValue(dt.Rows[i]["FLDITEM"].ToString().ToLower()).Selected = true;

                    if (cblextvetting.Items.FindByValue(dt.Rows[i]["FLDITEM"].ToString().ToLower()) != null)
                        cblextvetting.Items.FindByValue(dt.Rows[i]["FLDITEM"].ToString().ToLower()).Selected = true;


                }

            }

            
        }
    }
    protected void MenuTemplateMapping_TabStripCommand(object sender, EventArgs e)
    {
        try
        {
            DataListCommandEventArgs dce = (DataListCommandEventArgs)e;
            if (dce.CommandName.ToUpper().Equals("SAVE"))
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

             

              

                if (strcategoryid.Length > 1)
                {
                    strcategoryid.Remove(strcategoryid.Length - 1, 1);
                    PhoenixKPIRegisters.kpiscoringcriteriagroupinsert(General.GetNullableGuid(ViewState["CATEGORYGROUPID"].ToString())
                               , null
                               , null
                               , null
                               , strcategoryid.ToString()
                               , null);
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
