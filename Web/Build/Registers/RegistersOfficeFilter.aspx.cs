using System;
using System.Collections;
using System.Collections.Specialized;
using System.Configuration;
using System.Data;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using SouthNests.Phoenix.Common;
using SouthNests.Phoenix.Framework;
using SouthNests.Phoenix.Registers;
using System.Text;
using Telerik.Web.UI;

public partial class RegistersOfficeFilter : PhoenixBasePage
{
    protected void Page_Load(object sender, EventArgs e)
    {
        SessionUtil.PageAccessRights(this.ViewState);
        if (!IsPostBack)
        {
            PhoenixToolbar toolbar = new PhoenixToolbar();
            toolbar.AddButton("Go", "GO",ToolBarDirection.Right);

            MenuOfficeFilterMain.AccessRights = this.ViewState;
            MenuOfficeFilterMain.MenuList = toolbar.Show();
            txtName.Focus();

            cblAddressType.DataSource = PhoenixRegistersHard.ListHard(PhoenixSecurityContext.CurrentSecurityContext.UserCode,
                                                                      Convert.ToInt32(PhoenixHardTypeCode.ADDRESSTYPE));
            cblAddressType.DataBindings.DataTextField = "FLDHARDNAME";
            cblAddressType.DataBindings.DataValueField = "FLDHARDCODE";
            cblAddressType.DataBind();
            cblProductType.DataSource = PhoenixRegistersQuick.ListQuick(PhoenixSecurityContext.CurrentSecurityContext.UserCode, 
                                                                    Convert.ToInt32(PhoenixQuickTypeCode.GENERALPRODUCTTYPE));
            cblProductType.DataBindings.DataTextField = "FLDQUICKNAME";
            cblProductType.DataBindings.DataValueField = "FLDQUICKCODE";
            cblProductType.DataBind();

            cblAddressDepartment.DataSource = PhoenixRegistersDepartment.Listdepartment(PhoenixSecurityContext.CurrentSecurityContext.UserCode, null);
            cblAddressDepartment.DataBindings.DataTextField = "FLDDEPARTMENTNAME";
            cblAddressDepartment.DataBindings.DataValueField = "FLDDEPARTMENTID";
            cblAddressDepartment.DataBind();

            ucStatus.HardTypeCode = "191";

            ucQAGrading.QuickTypeCode = Convert.ToInt32(PhoenixQuickTypeCode.QAGRADING).ToString();

        }
    }

    protected void OfficeFilterMain_TabStripCommand(object sender, EventArgs e)
    {
        RadToolBarEventArgs dce = (RadToolBarEventArgs)e;
        string CommandName = ((RadToolBarButton)dce.Item).CommandName;

        string Script = "";
        Script += "<script language='JavaScript' id='BookMarkScript'>";
        Script += "fnReloadList();";
        Script += "</script>";

        if (CommandName.ToUpper().Equals("GO"))
        {
            ViewState["PAGENUMBER"] = 1;
            NameValueCollection criteria = new NameValueCollection();
            StringBuilder straddresstype = new StringBuilder();
            StringBuilder strproducttype = new StringBuilder();
            StringBuilder straddressdepartment = new StringBuilder();
            foreach (ButtonListItem item in cblAddressType.Items)
            {
                if (item.Selected == true)
                {
                    straddresstype.Append(item.Value.ToString());
                    straddresstype.Append(",");
                }
            }
            if (straddresstype.Length > 1)
            {
                straddresstype.Remove(straddresstype.Length - 1, 1);
            }
            foreach (ButtonListItem item in cblProductType.Items)
            {
                if (item.Selected == true)
                {
                    strproducttype.Append(item.Value.ToString());
                    strproducttype.Append(",");
                }
            }
            if (strproducttype.Length > 1)
            {
                strproducttype.Remove(strproducttype.Length - 1, 1);
            }

            foreach (ButtonListItem item in cblAddressDepartment.Items)
            {
                if (item.Selected == true)
                {
                    straddressdepartment.Append(item.Value.ToString());
                    straddressdepartment.Append(",");
                }
            }
            if (straddressdepartment.Length > 1)
            {
                straddressdepartment.Remove(straddressdepartment.Length - 1, 1);
            }
            criteria.Clear();

			criteria.Add("txtcode", txtcode.Text);
            criteria.Add("txtName", txtName.Text);
            criteria.Add("txtPhone1", txtPhone1.Text);
            criteria.Add("txtCity", txtCity.Text);
            criteria.Add("ucCountry", ucCountry.SelectedCountry);
            criteria.Add("txtPostalCode", txtPostalCode.Text);
            criteria.Add("txtEMail1", txtEMail.Text);
            criteria.Add("addresstype", straddresstype.ToString());
            criteria.Add("producttype", strproducttype.ToString());
            criteria.Add("status", ucStatus.SelectedHard);
            criteria.Add("qagrading", ucQAGrading.SelectedQuick);
            criteria.Add("txtBusinessProfile", txtBusinessProfile.Text);
            criteria.Add("addressdepartment", straddressdepartment.ToString());
            Filter.CurrentAddressFilterCriteria = criteria;
            RadScriptManager.RegisterStartupScript(Page, Page.GetType(), "BookMarkScript", Script, false);
        }
    }
}
