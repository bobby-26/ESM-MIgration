using System;
using System.Data;
using System.Web.UI.WebControls;
using SouthNests.Phoenix.StandardForm;
using SouthNests.Phoenix.Framework;
using Telerik.Web.UI;
using SouthNests.Phoenix.DocumentManagement;
using System.Text;
using SouthNests.Phoenix.Registers;


public partial class StandardFormDistributeVesselList : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {
            SessionUtil.PageAccessRights(this.ViewState);

            if (!IsPostBack)
            {
                ViewState["FORMID"] = "";
                ViewState["FORMNAME"] = "";

                PhoenixToolbar toolbar = new PhoenixToolbar();
                toolbar.AddButton("Distribute", "SAVE");
                MenuVesselMapping.AccessRights = this.ViewState;
                MenuVesselMapping.MenuList = toolbar.Show();

                if (Request.QueryString["FormId"] != null)
                {
                    ViewState["FORMID"] = Request.QueryString["FormId"].ToString();
                }

                if (Request.QueryString["FormName"] != null)
                {
                    ViewState["FORMNAME"] = Request.QueryString["FormName"].ToString();
                    txtFormName.Text = ViewState["FORMNAME"].ToString();
                }

                //BindVesselTypes();
                BindMapping();
                BindVesselTypeList();
                BindVesselList();
            }
        }
        catch (Exception ex)
        {

            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }
    protected void chkVesselTypeAll_Changed(object sender, EventArgs e)
    {
        try
        {
            if (chkVesselTypeAll.Checked == true)
            {
                foreach (ButtonListItem li in chkVesselType.Items)
                    li.Selected = true;

                foreach (ButtonListItem li in cblVessel.Items)
                    li.Selected = true;
            }
            else
            {
                foreach (ButtonListItem li in chkVesselType.Items)
                    li.Selected = false;

                foreach (ButtonListItem li in cblVessel.Items)
                    li.Selected = false;
            }

            //string script = "resizeDiv(divManual);resizeDiv(divFormCategory);resizeDiv(divJHACategory);resizeDiv(divVesselType);resizeDiv(divVesselType);resizeDiv(divVessel);\r\n;";
            //ScriptManager.RegisterStartupScript(this, typeof(Page), "BookmarkScript", script, true);
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
            return;
        }
    }
    protected string GetSelectedVesselType()
    {
        StringBuilder strvesseltype = new StringBuilder();
        foreach (ButtonListItem item in chkVesselType.Items)
        {
            if (item.Selected == true && item.Enabled == true)
            {
                strvesseltype.Append(item.Value.ToString());
                strvesseltype.Append(",");
            }
        }

        if (strvesseltype.Length > 1)
            strvesseltype.Remove(strvesseltype.Length - 1, 1);

        string vesseltype = strvesseltype.ToString();
        return vesseltype;
    }

    protected void BindVesselTypeList()
    {
        chkVesselType.Items.Clear();
        chkVesselType.DataSource = PhoenixRegistersHard.ListHard(PhoenixSecurityContext.CurrentSecurityContext.UserCode, 81);
        chkVesselType.DataBindings.DataTextField = "FLDHARDNAME";
        chkVesselType.DataBindings.DataValueField = "FLDHARDCODE";
        chkVesselType.DataBind();
    }

    protected void BindVesselList()
    {
        int? companyid = PhoenixSecurityContext.CurrentSecurityContext.CompanyID;

        cblVessel.Items.Clear();
        cblVessel.DataSource = PhoenixDocumentManagementDocument.DMSVesselList(null, companyid);
        cblVessel.DataBindings.DataTextField = "FLDVESSELNAME";
        cblVessel.DataBindings.DataValueField = "FLDVESSELID";
        cblVessel.DataBind();
    }

    protected void chkVesselType_Changed(object sender, EventArgs e)
    {
        string selectedcategorylist = GetSelectedVesselType();

        //ViewState["SelectedVesselList"] = "";
        foreach (ButtonListItem item in cblVessel.Items)
            item.Selected = false;

        int? companyid = PhoenixSecurityContext.CurrentSecurityContext.CompanyID;

        DataSet ds = PhoenixDocumentManagementDocument.DMSVesselList(selectedcategorylist, companyid);

        if (ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
        {
            foreach (DataRow dr in ds.Tables[0].Rows)
            {
                foreach (ButtonListItem item in cblVessel.Items)
                {
                    string vesselid = dr["FLDVESSELID"].ToString();

                    if (item.Value == vesselid && !item.Selected)
                    {
                        item.Selected = true;
                        break;
                    }
                }
            }
        }

        //string script = "resizeDiv(divManual);resizeDiv(divFormCategory);resizeDiv(divJHACategory);resizeDiv(divVesselType);resizeDiv(divVesselType);resizeDiv(divVessel);\r\n;";
        //ScriptManager.RegisterStartupScript(this, typeof(Page), "BookmarkScript", script, true);
    }
    protected void MenuVesselMapping_TabStripCommand(object sender, EventArgs e)
    {
        try
        {
            DataListCommandEventArgs dce = (DataListCommandEventArgs)e;
            if (dce.CommandName.ToUpper().Equals("SAVE"))
            {
                string mapping = ReadCheckBoxList(cblVessel);
                PhoenixFormBuilder.formDistribute(new Guid(ViewState["FORMID"].ToString()), mapping);

				ucStatus.Text = "Forms distributed to selected vessels";
                BindMapping();

                //ScriptManager.RegisterStartupScript(Page, Page.GetType(), "refreshScript", "javascript:fnReloadList('codehelp1',null,true);", true);
            }
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }

    private void BindCheckBoxList(RadCheckBoxList cbl, string list)
    {
        //foreach (string item in list.Split(','))
        //{
        //    if (item.Trim() != "")
        //    {
        //        if (cbl.Items.FindByValue(item) != null)
        //            cbl.Items.FindByValue(item).Selected = true;
        //    }
        //}
        StringBuilder strVessel = new StringBuilder();
        foreach (ButtonListItem item in cbl.Items)
        {
            if (item.Selected == true)
            {
                strVessel.Append(item.Value.ToString());
                strVessel.Append(",");
            }
        }
    }

    private string ReadCheckBoxList(RadCheckBoxList cbl)
    {
        string list = string.Empty;

        foreach (ButtonListItem item in cbl.Items)
        {
            if (item.Selected == true)
            {
                list = list + item.Value.ToString() + ",";
            }
        }
        list = list.TrimEnd(',');
        return list;
    }

    //private void BindVesselTypes()
    //{
    //    cblVessel.Items.Clear();
    //    DataSet ds = PhoenixFormBuilder.vesselList();
    //    cblVessel.DataSource = ds;
    //    cblVessel.DataTextField = "FLDVESSELNAME";
    //    cblVessel.DataValueField = "FLDVESSELID";
    //    cblVessel.DataBind();
    //}

    private void BindMapping()
    {
        DataTable dt = PhoenixFormBuilder.DistibutedVessels(new Guid(ViewState["FORMID"].ToString()));
        if (dt.Rows.Count > 0)
        {
            string mapping = dt.Rows[0]["FLDVESSELLIST"].ToString();
            BindCheckBoxList(cblVessel, mapping);
        }
    }

    //protected void SelectAll(object sender, EventArgs e)
    //{
    //    if (chkCheckAll.Checked == true)
    //    {
    //        foreach (ListItem item in cblVessel.Items)
    //        {
    //            item.Selected = true;
    //        }
    //    }
    //    else
    //    {
    //        foreach (ListItem item in cblVessel.Items)
    //        {
    //            item.Selected = false;
    //        }
    //    }
    //}
}
