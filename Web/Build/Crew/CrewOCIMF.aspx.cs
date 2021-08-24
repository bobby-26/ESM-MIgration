using System;
using System.Data;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using SouthNests.Phoenix.CrewManagement;
using SouthNests.Phoenix.CrewOperation;
using SouthNests.Phoenix.Framework;
using SouthNests.Phoenix.Registers;
using Telerik.Web.UI;
public partial class CrewOCIMF : PhoenixBasePage
{
    protected void Page_Load(object sender, EventArgs e)
    {
        SessionUtil.PageAccessRights(this.ViewState);
        PhoenixToolbar toolbar = new PhoenixToolbar();
        toolbar.AddImageButton("../Crew/CrewOCIMF.aspx?vslid=" + Request.QueryString["vslid"], "Find", "search.png", "FIND");
        MenuCrewOCIMF.AccessRights = this.ViewState;
        MenuCrewOCIMF.MenuList = toolbar.Show();

        PhoenixToolbar toolbar2 = new PhoenixToolbar();
        toolbar2.AddButton("Save", "SAVE", ToolBarDirection.Right);
        OcimfLogin.AccessRights = this.ViewState;
        OcimfLogin.MenuList = toolbar2.Show();

        if (!IsPostBack)
        {
            ViewState["vslid"] = string.Empty;
            ViewState["TOKENID"] = Request.QueryString["tokenid"];
            if (Request.QueryString["vslid"] != null && Request.QueryString["vslid"] != string.Empty)
            {
                lstVessel.SelectedVessel = Request.QueryString["vslid"];
                ViewState["vslid"] = Request.QueryString["vslid"];
            }
            if (Request.QueryString["uniqueid"] != null && Request.QueryString["uniqueid"] != string.Empty)
            {
                DataTable dt = PhoenixCrewOCIMF.OCIMFList(Request.QueryString["uniqueid"].ToString());
                if (dt.Rows.Count > 0)
                {
                    ViewState["TOKENID"] = dt.Rows[0]["FLDSECRETCODE"].ToString();
                }
            }
            ViewState["IDXDOC"] = string.Empty;
            Bindoilmajor();
            gvOCIMF.PageSize = int.Parse(PhoenixGeneralSettings.CurrentGeneralSetting.Records);
            gvSire.PageSize = int.Parse(PhoenixGeneralSettings.CurrentGeneralSetting.Records);
        }
    }
    protected void Rebind()
    {
        gvOCIMF.SelectedIndexes.Clear();
        gvOCIMF.EditIndexes.Clear();
        gvOCIMF.DataSource = null;
        gvOCIMF.Rebind();
        gvSire.SelectedIndexes.Clear();
        gvSire.EditIndexes.Clear();
        gvSire.DataSource = null;
        gvSire.Rebind();
    }
    protected void CrewOCIMF_TabStripCommand(object sender, EventArgs e)
    {
        RadToolBarEventArgs dce = (RadToolBarEventArgs)e;
        string CommandName = ((RadToolBarButton)dce.Item).CommandName;
        if (CommandName.ToUpper().Equals("FIND"))
        {
            Rebind();
            lstVessel_OnTextChanged(null, null);
        }
    }
    private void Bindoilmajor()
    {

        if (ViewState["vslid"] != null && ViewState["vslid"].ToString() != string.Empty)
        {
            DataTable dt = PhoenixRegistersOilMajorVesselMapping.ListOilmajormatrix(General.GetNullableInteger(ViewState["vslid"].ToString()), null);
            if (dt.Rows.Count > 0)
            {
                ddlOilMajor.SelectedHard = dt.Rows[0]["FLDOILMAJORID"].ToString();
                ViewState["MAPPINGID"] = dt.Rows[0]["FLDMAPPINGID"].ToString();
            }
            else
            {
                ddlOilMajor.SelectedHard = "";
                ViewState["MAPPINGID"] = null;
            }
        }
        else
        {
            ddlOilMajor.SelectedHard = "";
            ViewState["MAPPINGID"] = null;
        }
    }
    protected void lstVessel_OnTextChanged(object sender, EventArgs e)
    {
        if (lstVessel.SelectedVessel != "Dummy")
        {
            ViewState["vslid"] = lstVessel.SelectedVessel;
        }
        else
        {
            ViewState["vslid"] = null;
        }
        Rebind();
        Bindoilmajor();
    }
    protected void ddlOilMajor_TextChangedEvent(object sender, EventArgs e)
    {
        if (ViewState["vslid"] != null && ViewState["vslid"].ToString() != string.Empty)
        {
            DataTable dt = PhoenixRegistersOilMajorVesselMapping.ListOilmajormatrix(General.GetNullableInteger(ViewState["vslid"].ToString()), null);
            if (dt.Rows.Count > 0)
            {
                ViewState["MAPPINGID"] = dt.Rows[0]["FLDMAPPINGID"].ToString();
                Rebind();
            }
        }
    }
    protected void OcimfLoginTabs_TabStripCommand(object sender, EventArgs e)
    {
        try
        {
            RadToolBarEventArgs dce = (RadToolBarEventArgs)e;
            string CommandName = ((RadToolBarButton)dce.Item).CommandName;
            if (CommandName.ToUpper().Equals("SAVE"))
            {
                string mappingid = null;
                string vesselid = null;
                mappingid = (ViewState["MAPPINGID"] == null) ? null : (ViewState["MAPPINGID"].ToString());
                vesselid = (ViewState["vslid"] == null) ? null : (ViewState["vslid"].ToString());
                PhoenixRegistersOilMajorVesselMapping.CrewOilMajorVesselSave(General.GetNullableInteger(vesselid), General.GetNullableInteger(ddlOilMajor.SelectedHard), General.GetNullableInteger(mappingid));
                ucStatus.Text = "Updated Successfully";
            }
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }
    private void BindData()
    {
        if (ViewState["vslid"] != null && ViewState["vslid"].ToString() != string.Empty)
        {
            DataTable dt = PhoenixCrewOCIMF.OCIMFFetch(General.GetNullableInteger(ViewState["vslid"].ToString()), General.GetNullableInteger(ddlPrinciple.SelectedAddress));
            gvOCIMF.DataSource = dt;
            gvOCIMF.VirtualItemCount = dt.Rows.Count;
        }
    }
    private bool IsValidEvaluation(string itemid, string sortorder)
    {
        ucError.HeaderMessage = "Please provide the following required information";
        int result;
        if (int.TryParse(itemid, out result) == false)
            ucError.ErrorMessage = "Item Name is required.";

        if (sortorder.Trim().Equals(""))
            ucError.ErrorMessage = "Sort Order is required.";
        return (!ucError.IsError);
    }

    protected void BindSire()
    {
        try
        {
            if (ViewState["vslid"] != null && ViewState["vslid"].ToString() != string.Empty)
            {
                DataTable dt = PhoenixRegistersVessel.EditVessel(General.GetNullableInteger(ViewState["vslid"].ToString()).Value).Tables[0];
                DataTable dt1 = PhoenixCrewSireServiceOperations.GetCrewIndex(ViewState["TOKENID"].ToString(), dt.Rows[0]["FLDIMONUMBER"].ToString());
                DataTable sire = new DataTable();
                sire.Columns.Add("key");
                sire.Columns.Add("type");
                string[] cols = { "rank", "nationality", "certcomp", "issuecountry", "adminaccept"
                                        , "tankercert", "stcwvpara", "radioqual","yearsoperator","yearsrank","yearstankertype","yearsalltankertypes","datejoinedvessel","englishprof","yearswatch" };
                foreach (string str in cols)
                {
                    sire.Columns.Add(str);
                }
                if (dt1.Rows.Count > 0)
                {
                    ViewState["IDXDOC"] = dt1.Rows[0]["guid"].ToString();
                    DataSet ds = PhoenixCrewSireServiceOperations.GetCrewDocument(ViewState["TOKENID"].ToString(), new Guid(ViewState["IDXDOC"].ToString()));
                    DataTable sdt1 = ds.Tables[1];
                    DataTable sdt2 = ds.Tables[2];
                    foreach (DataRow dr in sdt1.Rows)
                    {
                        DataRow[] sdr = sdt2.Select("Crew_Id=" + dr["Crew_Id"].ToString());
                        DataRow temp = sire.NewRow();
                        temp["key"] = dr["key"].ToString();
                        temp["type"] = dr["type"].ToString();
                        foreach (DataRow dr1 in sdr)
                        {
                            temp[dr1["key"].ToString()] = dr1["Attribute_Text"].ToString();
                        }
                        sire.Rows.Add(temp);
                    }
                    gvSire.DataSource = sire;
                    gvSire.VirtualItemCount = sire.Rows.Count;
                }
            }

        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }

    protected void gvSire_ItemDataBound(Object sender, GridItemEventArgs e)
    {
        if (e.Item is GridEditableItem)
        {
            LinkButton db = (LinkButton)e.Item.FindControl("cmdDelete");
            if (db != null)
            {
                db.Attributes.Add("onclick", "return fnConfirmDelete(event); return false;");
                db.Visible = SessionUtil.CanAccess(this.ViewState, db.CommandName);
            }
        }
    }
    protected void gvSire_ItemCommand(object sender, GridCommandEventArgs e)
    {
        try
        {
            if (e.CommandName.ToUpper().Equals("SAVE"))
            {
                string keyid = ((RadLabel)e.Item.FindControl("lblKeyEdit")).Text;

                string rank = ((RadLabel)e.Item.FindControl("lblRankEdit")).Text;
                string crewtype = ((RadLabel)e.Item.FindControl("lblCrewTypeEdit")).Text;
                string nationality = ((RadLabel)e.Item.FindControl("lblNationalityEdit")).Text;
                string certcomp = ((RadTextBox)e.Item.FindControl("lblCertCompEdit")).Text;
                string issuecountry = ((RadLabel)e.Item.FindControl("lblIssuingCountryEdit")).Text;
                string admintaccpt = ((RadLabel)e.Item.FindControl("lblAdminAccptEdit")).Text;
                string tankercert = ((RadLabel)e.Item.FindControl("lblTankerCertEdit")).Text;
                string stcwpara = ((RadLabel)e.Item.FindControl("lblStcwParaEdit")).Text;
                string radioqual = ((RadLabel)e.Item.FindControl("lblRadioQualEdit")).Text;
                string yrsoperator = ((UserControlMaskNumber)e.Item.FindControl("txtYrsOperator")).Text;
                string yrsrank = ((UserControlMaskNumber)e.Item.FindControl("txtYrsRank")).Text;
                string tankertypeexp = ((UserControlMaskNumber)e.Item.FindControl("txtYrsTankerType")).Text;
                string alltankertypeexp = ((UserControlMaskNumber)e.Item.FindControl("txtYrsAllType")).Text;
                string joindate = DateTime.Parse(((UserControlDate)e.Item.FindControl("txtJoinDate")).Text).ToString("yyyy-MM-dd");
                string engprof = ((RadTextBox)e.Item.FindControl("txtEngProf")).Text;
                string YrsWatch = ((UserControlMaskNumber)e.Item.FindControl("txtYrsWatch")).Text;
                PhoenixCrewSireServiceOperations.UpdateCrewDocument(ViewState["TOKENID"].ToString(), new Guid(ViewState["IDXDOC"].ToString())
                , new Guid(keyid), crewtype, rank, nationality, certcomp, issuecountry, admintaccpt, tankercert, stcwpara, radioqual, yrsoperator
                , yrsrank, tankertypeexp, alltankertypeexp, joindate, engprof, YrsWatch);
                Rebind();
            }
            else if (e.CommandName.ToUpper().Equals("DELETE"))
            {

                string keyid = ((RadLabel)e.Item.FindControl("lblKey")).Text;
                string rank = ((RadLabel)e.Item.FindControl("lblRank")).Text;
                string crewtype = ((RadLabel)e.Item.FindControl("lblCrewType")).Text;
                string nationality = ((RadLabel)e.Item.FindControl("lblNationality")).Text;
                string certcomp = ((RadLabel)e.Item.FindControl("lblCertComp")).Text;
                string issuecountry = ((RadLabel)e.Item.FindControl("lblIssuingCountry")).Text;
                string admintaccpt = ((RadLabel)e.Item.FindControl("lblAdminAccpt")).Text;
                string tankercert = ((RadLabel)e.Item.FindControl("lblTankerCert")).Text;
                string stcwpara = ((RadLabel)e.Item.FindControl("lblStcwPara")).Text;
                string radioqual = ((RadLabel)e.Item.FindControl("lblRadioQual")).Text;
                string yrsoperator = ((RadLabel)e.Item.FindControl("lblYrsOperator")).Text;
                string yrsrank = ((RadLabel)e.Item.FindControl("lblYrsRank")).Text;
                string tankertypeexp = ((RadLabel)e.Item.FindControl("lblYrsTankerType")).Text;
                string alltankertypeexp = ((RadLabel)e.Item.FindControl("lblYrsAllType")).Text;
                string joindate = ((RadLabel)e.Item.FindControl("lblJoinDate")).Text;
                string engprof = ((RadLabel)e.Item.FindControl("lblEngProf")).Text;
                PhoenixCrewSireServiceOperations.DeleteCrewDocument(ViewState["TOKENID"].ToString(), new Guid(ViewState["IDXDOC"].ToString())
               , new Guid(keyid), crewtype, rank, nationality, certcomp, issuecountry, admintaccpt, tankercert, stcwpara, radioqual, yrsoperator
               , yrsrank, tankertypeexp, alltankertypeexp, joindate, engprof);
                Rebind();
            }

        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }
    protected void gvSire_NeedDataSource(object sender, GridNeedDataSourceEventArgs e)
    {
        try
        {
            BindSire();
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }
    protected void gvOCIMF_NeedDataSource(object sender, GridNeedDataSourceEventArgs e)
    {
        try
        {
            BindData();
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }
    protected void gvOCIMF_ItemCommand(object sender, GridCommandEventArgs e)
    {
        try
        {
            if (e.CommandName.ToUpper().Equals("SAVE"))
            {
                string rank = ((RadLabel)e.Item.FindControl("lblRank")).Text;
                string crewtype = ((RadLabel)e.Item.FindControl("lblCrewType")).Text;
                string nationality = ((RadLabel)e.Item.FindControl("lblNationality")).Text;
                string certcomp = ((RadLabel)e.Item.FindControl("lblCertComp")).Text;
                string issuecountry = ((RadLabel)e.Item.FindControl("lblIssuingCountry")).Text;
                string admintaccpt = ((RadLabel)e.Item.FindControl("lblAdminAccpt")).Text;
                string tankercert = ((RadLabel)e.Item.FindControl("lblTankerCert")).Text;
                string stcwpara = ((RadLabel)e.Item.FindControl("lblStcwPara")).Text;
                string radioqual = ((RadLabel)e.Item.FindControl("lblRadioQual")).Text;
                string yrsoperator = ((RadLabel)e.Item.FindControl("lblYrsOperator")).Text;
                string yrsrank = ((RadLabel)e.Item.FindControl("lblYrsRank")).Text;
                string tankertypeexp = ((RadLabel)e.Item.FindControl("lblYrsTankerType")).Text;
                string alltankertypeexp = ((RadLabel)e.Item.FindControl("lblYrsAllType")).Text;
                string joindate = ((RadLabel)e.Item.FindControl("lblJoinDate")).Text;
                string engprof = ((RadLabel)e.Item.FindControl("lblEngProf")).Text;
                string YearsWatch = ((RadLabel)e.Item.FindControl("lblYearsWatch")).Text;
                PhoenixCrewSireServiceOperations.InsertCrewDocument(ViewState["TOKENID"].ToString(), new Guid(ViewState["IDXDOC"].ToString())
                , crewtype, rank, nationality, certcomp, issuecountry, admintaccpt, tankercert, stcwpara, radioqual, yrsoperator
                , yrsrank, tankertypeexp, alltankertypeexp, joindate, engprof, YearsWatch);
                Rebind();
            }


        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }
    protected void gvOCIMF_ItemDataBound(Object sender, GridItemEventArgs e)
    {
        if (e.Item is GridEditableItem)
        {
            LinkButton db = (LinkButton)e.Item.FindControl("cmdAdd");
            if (db != null)
            {
                db.Attributes.Add("onclick", "return fnConfirmDelete(event, 'Are you sure you want to Upload this data to SIRE ?'); return false;");
                db.Visible = SessionUtil.CanAccess(this.ViewState, db.CommandName);
            }
        }
    }
}
