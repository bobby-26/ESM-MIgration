using System;
using System.Collections.Specialized;
using System.Text;
using System.Web.UI;
using System.Web.UI.WebControls;
using SouthNests.Phoenix.Framework;
using SouthNests.Phoenix.Registers;
using Telerik.Web.UI;

public partial class RegistersLicenceCostComponents : PhoenixBasePage
{
    protected void Page_Load(object sender, EventArgs e)
    {
        SessionUtil.PageAccessRights(this.ViewState);

        PhoenixToolbar toolbar = new PhoenixToolbar();

        toolbar.AddButton("Ok", "GO",ToolBarDirection.Right);
        MenuLicenceCostComponents.AccessRights = this.ViewState;
        MenuLicenceCostComponents.MenuList = toolbar.Show();

        if (!IsPostBack)
        {   
            BindCheckBoxList();
        }
    }

    protected void BindCheckBoxList()
    {
        cblLicenceComponents.DataSource = PhoenixRegistersHard.ListHard(PhoenixSecurityContext.CurrentSecurityContext.UserCode, 120);
        cblLicenceComponents.DataBindings.DataTextField = "FLDHARDNAME";
        cblLicenceComponents.DataBindings.DataValueField = "FLDHARDCODE";
        cblLicenceComponents.DataBind();
    }

    protected void LicenceCostComponents_TabStripCommand(object sender, EventArgs e)
    {
        RadToolBarEventArgs dce = (RadToolBarEventArgs)e;
        string CommandName = ((RadToolBarButton)dce.Item).CommandName;

        try
        {
            if (CommandName.ToUpper().Equals("GO"))
            {
                StringBuilder strLicenceComponentsId = new StringBuilder();
                StringBuilder strLicenceComponents = new StringBuilder();

                foreach (ButtonListItem item in cblLicenceComponents.Items)
                {
                    if (item.Selected == true)
                    {
                        strLicenceComponentsId.Append(item.Value.ToString());
                        strLicenceComponentsId.Append(",");

                        strLicenceComponents.Append(item.Text.ToString());
                        strLicenceComponents.Append(" + ");
                    }
                }

                if (strLicenceComponentsId.Length > 1)
                {
                    strLicenceComponentsId.Remove(strLicenceComponentsId.Length - 1, 1);
                    strLicenceComponents.Remove(strLicenceComponents.Length - 3, 3);
                }

                if (IsValidLicenceComponent(strLicenceComponentsId.ToString()))
                {
                    string Script = "";

                    NameValueCollection nvc = new NameValueCollection();

                    nvc = Filter.CurrentPickListSelection;

                    nvc.Set(nvc.GetKey(1), strLicenceComponentsId.ToString());
                    nvc.Set(nvc.GetKey(2), strLicenceComponents.ToString());

                    Script += "<script language='javaScript' id='BookMarkScript'>" + "\n";
                    Script += "fnClosePickList('codehelp1','filterandsearch');";
                    Script += "</script>" + "\n";

                    Filter.CurrentPickListSelection = nvc;

                    Page.ClientScript.RegisterStartupScript(typeof(Page), "BookMarkScript", Script);
                }
                else
                {
                    ucError.Visible = true;
                    return;
                }
            }
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }

    private bool IsValidLicenceComponent(string licenceComponents)
    {
        ucError.HeaderMessage = "Please provide the following required information";

        if (licenceComponents.Trim().Length == 0)
            ucError.ErrorMessage = "Select Atleast one Component.";

        return (!ucError.IsError);
    }

    private void Reset()
    {
        foreach (ButtonListItem item in cblLicenceComponents.Items)
        {
            item.Selected = false;
        }
    }

    //protected void SelectAllComponents(object sender, EventArgs e)
    //{
    //    if (cblLicenceComponents.Checked == true)
    //    {
    //        foreach (ListItem item in cblLicenceComponents.Items)
    //        {
    //            item.Selected = true;
    //        }
    //    }
    //    else
    //    {
    //        foreach (ListItem item in cblLicenceComponents.Items)
    //        {
    //            item.Selected = false;
    //        }
    //    }
    //}
}
