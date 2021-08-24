using System;
using System.Collections.Specialized;
using System.Text;
using System.Web.UI;
using System.Web.UI.WebControls;
using SouthNests.Phoenix.Common;
using SouthNests.Phoenix.Framework;
using SouthNests.Phoenix.Registers;
using Telerik.Web.UI;
public partial class PreSeaOfficeFilter : PhoenixBasePage
{
    protected void Page_Load(object sender, EventArgs e)
    {
        SessionUtil.PageAccessRights(this.ViewState);
        PhoenixToolbar toolbar = new PhoenixToolbar();
        toolbar.AddButton("Go", "GO");

        MenuOfficeFilterMain.AccessRights = this.ViewState;
        MenuOfficeFilterMain.MenuList = toolbar.Show();
        if (!IsPostBack)
        {          
            txtName.Focus();

            cblAddressType.DataSource = PhoenixRegistersHard.ListHard(PhoenixSecurityContext.CurrentSecurityContext.UserCode,
                                                                      Convert.ToInt32(PhoenixHardTypeCode.ADDRESSTYPE), 0, "TRI,SCL,CLG");
            cblAddressType.DataTextField = "FLDHARDNAME";
            cblAddressType.DataValueField = "FLDHARDCODE";
            cblAddressType.DataBind();

            cblAddressDepartment.DataSource = PhoenixRegistersDepartment.Listdepartment(PhoenixSecurityContext.CurrentSecurityContext.UserCode, null);
            cblAddressDepartment.DataTextField = "FLDDEPARTMENTNAME";
            cblAddressDepartment.DataValueField = "FLDDEPARTMENTID";
            cblAddressDepartment.DataBind();

            ucStatus.HardTypeCode = "191";
            ucQAGrading.QuickTypeCode = Convert.ToInt32(PhoenixQuickTypeCode.QAGRADING).ToString();

        }
    }

    protected void OfficeFilterMain_TabStripCommand(object sender, EventArgs e)
    {
        DataListCommandEventArgs dce = (DataListCommandEventArgs)e;

        string Script = "";
        Script += "<script language=JavaScript id='BookMarkScript'>" + "\n";
        Script += "fnReloadList();";
        Script += "</script>" + "\n";

        if (dce.CommandName.ToUpper().Equals("GO"))
        {
            ViewState["PAGENUMBER"] = 1;
            NameValueCollection criteria = new NameValueCollection();
            StringBuilder straddresstype = new StringBuilder();
            StringBuilder straddressdepartment = new StringBuilder();
            foreach (ListItem item in cblAddressType.Items)
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

            foreach (ListItem item in cblAddressDepartment.Items)
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
           // criteria.Add("ucState", );
            criteria.Add("ucCountry", ucCountry.SelectedCountry);
            criteria.Add("txtPostalCode", txtPostalCode.Text);
            criteria.Add("txtEMail1", txtEMail.Text);
            criteria.Add("addresstype", straddresstype.ToString());
            criteria.Add("producttype", "");
            criteria.Add("status", ucStatus.SelectedHard);
            criteria.Add("qagrading", ucQAGrading.SelectedQuick);
            criteria.Add("txtBusinessProfile", txtBusinessProfile.Text);
            criteria.Add("addressdepartment", straddressdepartment.ToString());
            //criteria.Add("pagenumber", ViewState["PAGENUMBER"].ToString());
            Filter.CurrentPreSeaAddressFilterCriteria = criteria;
        }

        Page.ClientScript.RegisterStartupScript(typeof(Page), "BookMarkScript", Script);
    }
}
