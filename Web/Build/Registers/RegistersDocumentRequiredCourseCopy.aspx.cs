using System;
using System.Text;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using SouthNests.Phoenix.Common;
using SouthNests.Phoenix.Framework;
using SouthNests.Phoenix.Registers;
using System.Data;
using Telerik.Web.UI;

public partial class RegistersDocumentRequiredCourseCopy : PhoenixBasePage
{
    protected void Page_Load(object sender, EventArgs e)
    {
        SessionUtil.PageAccessRights(this.ViewState);

        PhoenixToolbar toolbar = new PhoenixToolbar();

        toolbar.AddButton("Copy", "COPY", ToolBarDirection.Right);
        MenuCopy.AccessRights = this.ViewState;
        MenuCopy.MenuList = toolbar.Show();
     
        if (!IsPostBack)
        {
            ucVesselType.VesselTypeList = PhoenixRegistersVesselType.ListVesselType(0);
            ucVesselType.DataBind();

            ViewState["PAGENUMBER"] = 1;
            ViewState["SORTEXPRESSION"] = null;
            ViewState["SORTDIRECTION"] = null;
            ViewState["CURRENTINDEX"] = 1;
            ucPrincipal.AddressType = ((int)PhoenixAddressType.PRINCIPAL).ToString();
            DataSet dsVessel = PhoenixRegistersVessel.EditVessel(Int16.Parse(Filter.CurrentVesselMasterFilter));
            DataRow drVessel = dsVessel.Tables[0].Rows[0];
            ucFlag.SelectedFlag = drVessel["FLDFLAG"].ToString();
            ucVesselType.SelectedVesseltype = drVessel["FLDTYPE"].ToString();
            ucPrincipal.SelectedAddress = drVessel["FLDPRINCIPALNAME"].ToString();
            currentFlag.Text = drVessel["FLDFLAGNAME"].ToString();
            currentVesselType.SelectedVesseltype = drVessel["FLDTYPE"].ToString();
            currentFlag.ReadOnly = true;
            currentVesselType.Enabled = false;
           

            ViewState["vesseltype"] = ucVesselType.SelectedVesseltype;
            BindRank();
            BindVessel(null, null);
            gvDocumentsRequired.PageSize = int.Parse(PhoenixGeneralSettings.CurrentGeneralSetting.Records);
        }
    }

    protected void BindRank()
    {
        if (Request.QueryString["rank"] != null)
        {
            if (Request.QueryString["rank"].ToString() != "Dummy" && Request.QueryString["rank"].ToString() != "")
            {
                DataSet ds = PhoenixRegistersRank.EditRank(int.Parse(Request.QueryString["rank"]));
                ucRank.SelectedRank = ds.Tables[0].Rows[0]["FLDRANKID"].ToString();
            }
        }

    }

    protected void BindVessel(object sender, EventArgs e)
    {
        ViewState["flag"] = ucFlag.SelectedFlag;

        if (!IsPostBack)
        {
            ViewState["flag"] = Request.QueryString["flag"].ToString();
        }
        if (IsPostBack)
        {
            ViewState["vesseltype"] = ucVesselType.SelectedVesseltype;
        }
        cblVessel.DataSource = "";
        cblVessel.DataTextField = "";
        cblVessel.DataValueField = "";

        cblVessel.DataSource = PhoenixRegistersVessel.ListVessel(
        General.GetNullableInteger(ViewState["flag"].ToString()), ucPrincipal.SelectedAddress == "Dummy" ? "" : ucPrincipal.SelectedAddress, 1,
        ViewState["vesseltype"].ToString());
        cblVessel.DataTextField = "FLDVESSELNAME";
        cblVessel.DataValueField = "FLDVESSELID";
        cblVessel.DataBind();
        Rebind();
    }
    protected void Rebind()
    {
        gvDocumentsRequired.SelectedIndexes.Clear();
        gvDocumentsRequired.EditIndexes.Clear();
        gvDocumentsRequired.DataSource = null;
        gvDocumentsRequired.Rebind();
    }
    protected void Copy_TabStripCommand(object sender, EventArgs e)
    {
        RadToolBarEventArgs dce = (RadToolBarEventArgs)e;
        string CommandName = ((RadToolBarButton)dce.Item).CommandName;
        int CurrentVessel = Convert.ToInt32(Filter.CurrentVesselMasterFilter);


        if (CommandName.ToUpper().Equals("COPY"))
        {
            StringBuilder strVessel = new StringBuilder();

            foreach (ListItem item in cblVessel.Items)
            {
                if (item.Selected == true)
                {
                    if (item.Value.ToString() != CurrentVessel.ToString())
                    {
                        strVessel.Append(item.Value.ToString());
                        strVessel.Append(",");
                    }
                    else
                    {
                        ucError.ErrorMessage = "Cannot Copy course to same vessel, Remove Current Vessel";
                        ucError.Visible = true;
                        return;
                    }
                }
            }
            if (strVessel.Length > 1)
            {
                strVessel.Remove(strVessel.Length - 1, 1);
            }


            if (IsValidCopy(strVessel.ToString()))
            {

                PhoenixRegistersVesselDocumentCourse.CopyDocumentsRequired(
                     strVessel.ToString()
                     , Convert.ToInt32(Request.QueryString["flag"].ToString())
                     , General.GetNullableInteger(ucDocumentType.SelectedHard == "Dummy" ? "" : ucDocumentType.SelectedHard)
                     , Convert.ToInt32(ViewState["vesseltype"].ToString())
                     , General.GetNullableInteger(ucRank.SelectedRank == "Dummy" ? "" : ucRank.SelectedRank)
                     , Convert.ToInt32(ucFlag.SelectedFlag)
                     , Convert.ToInt32(Filter.CurrentVesselMasterFilter)
                     , PhoenixSecurityContext.CurrentSecurityContext.UserCode);

                ucStatus.Text = "Course Details successfully copied";
                ucStatus.Visible = true;
            }
            else
            {
                ucError.Visible = true;
                return;
            }
        }
    }
    private bool IsValidCopy(string strVessel)
    {
        ucError.HeaderMessage = "Please provide the following required information";

        if (strVessel.Trim().Equals(""))
            ucError.ErrorMessage = "Select atleast one vessel to copy.";

        if (General.GetNullableInteger(Request.QueryString["vesseltype"].ToString()) == null)
            ucError.ErrorMessage = "Copying from Vessel type is not specified.";


        if (ucFlag.SelectedFlag == "Dummy")
        {
            ucError.ErrorMessage = "Flag is Required";
        }

        if (ucVesselType.SelectedVesseltype == "Dummy")
        {
            ucError.ErrorMessage = "Vessel Type is Required";
        }

        return (!ucError.IsError);
    }
    protected void gvDocumentsRequired_ItemCommand(object sender, GridCommandEventArgs e)
    {
        try
        {
            if (e.CommandName == "Page")
            {
                ViewState["PAGENUMBER"] = null;
            }
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }
    protected void gvDocumentsRequired_NeedDataSource(object sender, GridNeedDataSourceEventArgs e)
    {
        try
        {
            ViewState["PAGENUMBER"] = ViewState["PAGENUMBER"] != null ? ViewState["PAGENUMBER"] : gvDocumentsRequired.CurrentPageIndex + 1;
            BindData();
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }

    protected void SelectAllVessel(object sender, EventArgs e)
    {
        if (chkChkAllVessel.Checked == true)
        {
            foreach (ListItem item in cblVessel.Items)
            {
                item.Selected = true;
            }
        }
        else
        {
            foreach (ListItem item in cblVessel.Items)
            {
                item.Selected = false;
            }
        }
    }
    private void BindData()
    {
        int iRowCount = 0;
        int iTotalPageCount = 0;

        string sortexpression = (ViewState["SORTEXPRESSION"] == null) ? null : (ViewState["SORTEXPRESSION"].ToString());
        int? sortdirection = null;
        if (ViewState["SORTDIRECTION"] != null)
            sortdirection = Int32.Parse(ViewState["SORTDIRECTION"].ToString());

        ViewState["Rank"] = ucRank.SelectedRank;

        if (!IsPostBack)
        {
            if (Request.QueryString["rank"] != null)
            {
                if (Request.QueryString["rank"] != "Dummy")
                {

                    ViewState["Rank"] = Request.QueryString["rank"];
                }
            }
        }
        DataSet ds = PhoenixRegistersVesselDocumentCourse.CopyDocumentsRequiredSearch(General.GetNullableInteger(Filter.CurrentVesselMasterFilter),
                General.GetNullableInteger(ViewState["Rank"].ToString()), General.GetNullableInteger(currentFlag.Text),
                General.GetNullableInteger(currentVesselType.SelectedVesseltype), General.GetNullableInteger(ucDocumentType.SelectedHard),
                sortexpression, sortdirection, Int32.Parse(ViewState["PAGENUMBER"].ToString()), gvDocumentsRequired.PageSize, ref iRowCount, ref iTotalPageCount);
        gvDocumentsRequired.DataSource = ds;
        gvDocumentsRequired.VirtualItemCount = iRowCount;
        ViewState["ROWCOUNT"] = iRowCount;
        ViewState["TOTALPAGECOUNT"] = iTotalPageCount;
    }
    

}
