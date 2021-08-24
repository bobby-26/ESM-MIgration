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

public partial class CrewOCIMFDetail : PhoenixBasePage
{
    protected void Page_Load(object sender, EventArgs e)
    {
        SessionUtil.PageAccessRights(this.ViewState);
        PhoenixToolbar toolbar = new PhoenixToolbar();
        toolbar.AddImageButton("../Crew/CrewOCIMFDetail.aspx?vslid=" + Request.QueryString["vslid"], "Find", "search.png", "FIND");
        MenuCrewOCIMF.AccessRights = this.ViewState;
        MenuCrewOCIMF.MenuList = toolbar.Show();

        PhoenixToolbar toolbar2 = new PhoenixToolbar();
        toolbar2.AddButton("OCIMF", "OCIMF");
        toolbar2.AddButton("Login", "LOGIN");
        OcimfLogin.AccessRights = this.ViewState;      
        OcimfLogin.MenuList = toolbar2.Show();
        OcimfLogin.SelectedMenuIndex = 0;


        PhoenixToolbar toolbarocimf = new PhoenixToolbar();
        toolbarocimf = new PhoenixToolbar();
        toolbarocimf.AddButton("Save", "SAVE");
        Menuocimf.AccessRights = this.ViewState;
        Menuocimf.MenuList = toolbarocimf.Show();


        if (!IsPostBack)
        {
            if (Request.QueryString["vslid"] != null && Request.QueryString["vslid"] != string.Empty)
                lstVessel.SelectedVessel = Request.QueryString["vslid"];

            ViewState["IDXDOC"] = string.Empty;
            BindVessel();
            Bindoilmajor();

        }
        BindData();
        BindSire();
    }


    private void Bindoilmajor()
    {

        if (Request.QueryString["vslid"] != null)
        {
            DataTable dt = PhoenixRegistersOilMajorVesselMapping.ListOilmajormatrix(General.GetNullableInteger(Request.QueryString["vslid"].ToString()), null);

            if (dt.Rows.Count > 0)
            {
                ddlOilMajor.SelectedHard = dt.Rows[0]["FLDOILMAJORID"].ToString();

                ViewState["MAPPINGID"]  = dt.Rows[0]["FLDMAPPINGID"].ToString();
            }

        }
    }

    protected void CrewOCIMF_TabStripCommand(object sender, EventArgs e)
    {
        DataListCommandEventArgs dce = (DataListCommandEventArgs)e;
        if (dce.CommandName.ToUpper().Equals("FIND"))
        {
            BindData();
            lstVessel_OnTextChanged(null, null);
        }
    }

    protected void OcimfLoginTabs_TabStripCommand(object sender, EventArgs e)
    {
        try
        {
            DataListCommandEventArgs dce = (DataListCommandEventArgs)e;

           if (dce.CommandName.ToUpper().Equals("LOGIN"))
           {
                if (Request.QueryString["vslid"] != null)
                    Response.Redirect("CrewOCIMFLogin.aspx?vslid=" + Request.QueryString["vslid"], false);
           }         
           
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }

    protected void Menuocimf_TabStripCommand(object sender, EventArgs e)
    {
        try
        {
            DataListCommandEventArgs dce = (DataListCommandEventArgs)e;

           if (dce.CommandName.ToUpper().Equals("SAVE"))
            {

                string mappingid = null;

                mappingid = (ViewState["MAPPINGID"] == null) ? null : (ViewState["MAPPINGID"].ToString());

                PhoenixRegistersOilMajorVesselMapping.CrewOilMajorVesselSave(General.GetNullableInteger(Request.QueryString["vslid"].ToString()), General.GetNullableInteger(ddlOilMajor.SelectedHard), General.GetNullableInteger(mappingid));

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

        DataTable dt = PhoenixCrewOCIMF.OCIMFFetch(General.GetNullableInteger(Request.QueryString["vslid"]), General.GetNullableInteger(ddlPrinciple.SelectedAddress));

        if (dt.Rows.Count > 0)
        {
            gvOCIMF.DataSource = dt;
            gvOCIMF.DataBind();

        }
        else
        {
            ShowNoRecordsFound(dt, gvOCIMF);
        }

    }

    protected void gvOCIMF_RowCommand(object sender, GridViewCommandEventArgs e)
    {
        try
        {
            if (e.CommandName.ToUpper().Equals("SORT"))
                return;

            GridView _gridView = (GridView)sender;
            int nCurrentRow = Int32.Parse(e.CommandArgument.ToString());


            if (e.CommandName.ToUpper().Equals("ADD"))
            {

                string rank = ((Label)_gridView.Rows[nCurrentRow].FindControl("lblRankName")).Text;
                string crewtype = ((Label)_gridView.Rows[nCurrentRow].FindControl("lblCrewType")).Text;
                string nationality = ((Label)_gridView.Rows[nCurrentRow].FindControl("lblNationality")).Text;
                string certcomp = ((Label)_gridView.Rows[nCurrentRow].FindControl("lblCertComp")).Text;
                string issuecountry = ((Label)_gridView.Rows[nCurrentRow].FindControl("lblIssuingCountry")).Text;
                string admintaccpt = ((Label)_gridView.Rows[nCurrentRow].FindControl("lblAdminAccpt")).Text;
                string tankercert = ((Label)_gridView.Rows[nCurrentRow].FindControl("lblTankerCert")).Text;
                string stcwpara = ((Label)_gridView.Rows[nCurrentRow].FindControl("lblStcwPara")).Text;
                string radioqual = ((Label)_gridView.Rows[nCurrentRow].FindControl("lblRadioQual")).Text;
                string yrsoperator = ((Label)_gridView.Rows[nCurrentRow].FindControl("lblYrsOperator")).Text;
                string yrsrank = ((Label)_gridView.Rows[nCurrentRow].FindControl("lblYrsRank")).Text;
                string tankertypeexp = ((Label)_gridView.Rows[nCurrentRow].FindControl("lblYrsTankerType")).Text;
                string alltankertypeexp = ((Label)_gridView.Rows[nCurrentRow].FindControl("lblYrsAllType")).Text;
                string joindate = ((Label)_gridView.Rows[nCurrentRow].FindControl("lblJoinDate")).Text;
                string engprof = ((Label)_gridView.Rows[nCurrentRow].FindControl("lblEngProf")).Text;
                string YearsWatch = ((Label)_gridView.Rows[nCurrentRow].FindControl("lblYearsWatch")).Text;

                if (ViewState["IDXDOC"] != null)
                {
                    PhoenixCrewOCIMF.InsertCrewDocument(new Guid(ViewState["IDXDOC"].ToString()), crewtype, rank, nationality, certcomp, issuecountry, admintaccpt, tankercert, stcwpara, radioqual
                    , decimal.Parse(yrsoperator), decimal.Parse(yrsrank), decimal.Parse(tankertypeexp), decimal.Parse(alltankertypeexp), DateTime.Parse(joindate), engprof, decimal.Parse(YearsWatch));

                    ucStatus.Text = "Crew document scheduled to add";
                }

                BindData();
                BindSire();
            }
        }

        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }



    protected void gvOCIMF_RowDataBound(Object sender, System.Web.UI.WebControls.GridViewRowEventArgs e)
    {
        if (e.Row.RowType == DataControlRowType.Header)
        {
            if (ViewState["SORTEXPRESSION"] != null)
            {
                HtmlImage img = (HtmlImage)e.Row.FindControl(ViewState["SORTEXPRESSION"].ToString());
                if (img != null)
                {
                    if (ViewState["SORTDIRECTION"] == null || ViewState["SORTDIRECTION"].ToString() == "0")
                        img.Src = Session["images"] + "/arrowUp.png";
                    else
                        img.Src = Session["images"] + "/arrowDown.png";

                    img.Visible = true;
                }
            }
        }
        if (e.Row.RowType == DataControlRowType.DataRow)
        {
            ImageButton del = (ImageButton)e.Row.FindControl("cmdAdd");
            if (del != null)
            {
                del.Visible = SessionUtil.CanAccess(this.ViewState, del.CommandName);
                del.Attributes.Add("onclick", "return fnConfirmDelete(event, 'Are you sure you want to Upload this data to SIRE ?'); return false;");
            }
        }

    }

    protected void lstVessel_OnTextChanged(object sender, EventArgs e)
    {
        BindSire();
    }

    protected void BindSire()
    {
        try
        {
            DataTable dt = PhoenixCrewOCIMF.OCIMFSireFetch(General.GetNullableInteger(Request.QueryString["vslid"]));
            if (dt.Rows.Count > 0)
            {
                gvSire.DataSource = dt;
                gvSire.DataBind();
            }
            else
            {
                ShowNoRecordsFound(dt, gvSire);
            }
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }

    protected void BindVessel()
    {
        try
        {
            DataTable dt = PhoenixCrewOCIMF.OCIMFVesselList(General.GetNullableInteger(Request.QueryString["vslid"]));

            if (dt.Rows.Count > 0)
            {
                ViewState["IDXDOC"] = dt.Rows[0]["FLDDOCGUID"].ToString();
            }
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }


    protected void gvSire_RowDataBound(Object sender, GridViewRowEventArgs e)
    {
        if (e.Row.RowType == DataControlRowType.Header)
        {
            if (ViewState["SORTEXPRESSION"] != null)
            {
                HtmlImage img = (HtmlImage)e.Row.FindControl(ViewState["SORTEXPRESSION"].ToString());
                if (img != null)
                {
                    if (ViewState["SORTDIRECTION"] == null || ViewState["SORTDIRECTION"].ToString() == "0")
                        img.Src = Session["images"] + "/arrowUp.png";
                    else
                        img.Src = Session["images"] + "/arrowDown.png";

                    img.Visible = true;
                }
            }
        }

        if (e.Row.RowType == DataControlRowType.DataRow)
        {

            Label lblUpdated = (Label)e.Row.FindControl("lblUpdated");
            Label lblDeleted = (Label)e.Row.FindControl("lblDeleted");
            Image ImgUpdated = (Image)e.Row.FindControl("ImgUpdated");
            Image ImgDeleted = (Image)e.Row.FindControl("ImgDeleted");

            if (lblUpdated != null && lblUpdated.Text == "1")
            {
                if (ImgUpdated != null)
                    ImgUpdated.Visible = true;
            }
            if (lblDeleted != null && lblDeleted.Text == "1")
            {
                if (ImgDeleted != null)
                    ImgDeleted.Visible = true;
            }

            ImageButton del = (ImageButton)e.Row.FindControl("cmdDelete");
            if (del != null)
            {
                del.Visible = SessionUtil.CanAccess(this.ViewState, del.CommandName);
                del.Attributes.Add("onclick", "return fnConfirmDelete(event, 'Are you sure you want to delete SIRE data ?'); return false;");
            }
        }
    }
    protected void gvSire_RowEditing(object sender, GridViewEditEventArgs de)
    {

        GridView _gridView = (GridView)sender;
        _gridView.EditIndex = de.NewEditIndex;
        _gridView.SelectedIndex = de.NewEditIndex;
        BindSire();
    }
    protected void gvSire_RowCancelingEdit(object sender, GridViewCancelEditEventArgs e)
    {
        GridView _gridView = (GridView)sender;
        _gridView.EditIndex = -1;
        _gridView.SelectedIndex = e.RowIndex;
        BindSire();
    }


    protected void gvSire_RowUpdating(object sender, GridViewUpdateEventArgs e)
    {
        try
        {
            GridView _gridView = (GridView)sender;
            int nCurrentRow = e.RowIndex;

            string keyid = ((Label)_gridView.Rows[nCurrentRow].FindControl("lblCrewkeyEdit")).Text;
            string rank = ((Label)_gridView.Rows[nCurrentRow].FindControl("lblRankEdit")).Text;
            string crewtype = ((Label)_gridView.Rows[nCurrentRow].FindControl("lblCrewTypeEdit")).Text;
            string nationality = ((Label)_gridView.Rows[nCurrentRow].FindControl("lblNationalityEdit")).Text;
            string certcomp = ((Label)_gridView.Rows[nCurrentRow].FindControl("lblCertCompEdit")).Text;
            string issuecountry = ((Label)_gridView.Rows[nCurrentRow].FindControl("lblIssuingCountryEdit")).Text;
            string admintaccpt = ((Label)_gridView.Rows[nCurrentRow].FindControl("lblAdminAccptEdit")).Text;
            string tankercert = ((Label)_gridView.Rows[nCurrentRow].FindControl("lblTankerCertEdit")).Text;
            string stcwpara = ((Label)_gridView.Rows[nCurrentRow].FindControl("lblStcwParaEdit")).Text;
            string radioqual = ((Label)_gridView.Rows[nCurrentRow].FindControl("lblRadioQualEdit")).Text;
            string yrsoperator = ((UserControlMaskNumber)_gridView.Rows[nCurrentRow].FindControl("txtYrsOperator")).Text;
            string yrsrank = ((UserControlMaskNumber)_gridView.Rows[nCurrentRow].FindControl("txtYrsRank")).Text;
            string tankertypeexp = ((UserControlMaskNumber)_gridView.Rows[nCurrentRow].FindControl("txtYrsTankerType")).Text;
            string alltankertypeexp = ((UserControlMaskNumber)_gridView.Rows[nCurrentRow].FindControl("txtYrsAllType")).Text;
            string joindate = ((UserControlDate)_gridView.Rows[nCurrentRow].FindControl("txtJoinDate")).Text;
            string engprof = ((TextBox)_gridView.Rows[nCurrentRow].FindControl("txtEngProf")).Text;
            string YrsWatch = ((UserControlMaskNumber)_gridView.Rows[nCurrentRow].FindControl("txtYrsWatch")).Text;

            if (ViewState["IDXDOC"] != null)
            {
                if (!IsValidate(yrsoperator, yrsrank, tankertypeexp, alltankertypeexp, joindate, engprof, YrsWatch))
                {
                    ucError.Visible = true;
                    return;
                }

                PhoenixCrewOCIMF.UpdateCrewDocument(new Guid(ViewState["IDXDOC"].ToString()), General.GetNullableGuid(keyid), crewtype, rank, nationality, certcomp, issuecountry, admintaccpt, tankercert, stcwpara, radioqual
                , decimal.Parse(yrsoperator), decimal.Parse(yrsrank), decimal.Parse(tankertypeexp), decimal.Parse(alltankertypeexp), DateTime.Parse(joindate), engprof, decimal.Parse(YrsWatch));

                ucStatus.Text = "Crew document scheduled to update";
            }
            _gridView.EditIndex = -1;
            BindSire();

        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }


    private bool IsValidate(string yrsoperator, string yrsrank, string tankertypeexp, string alltankertypeexp, string joindate, string engprof, string YrsWatch)
    {
        ucError.HeaderMessage = "Please provide the following required information";

        if (General.GetNullableDecimal(yrsoperator) == null)
            ucError.ErrorMessage = "Operator is required";

        if (General.GetNullableDecimal(yrsrank) == null)
            ucError.ErrorMessage = "Rank is required";

        if (General.GetNullableDecimal(tankertypeexp) == null)
            ucError.ErrorMessage = "Tanker Type is required";

        if (General.GetNullableDecimal(alltankertypeexp) == null)
            ucError.ErrorMessage = "All Types is required";

        if (General.GetNullableDateTime(joindate) == null)
            ucError.ErrorMessage = "Sign-On Date is required";

        if (General.GetNullableString(engprof) == null)
            ucError.ErrorMessage = "English prof is required";

        if (General.GetNullableDecimal(YrsWatch) == null)
            ucError.ErrorMessage = "Years Watch is required";


        return (!ucError.IsError);
    }


    protected void gvSire_RowDeleting(object sender, GridViewDeleteEventArgs de)
    {
        try
        {
            GridView _gridView = (GridView)sender;
            int nCurrentRow = de.RowIndex;

            string keyid = ((Label)_gridView.Rows[nCurrentRow].FindControl("lblCrewkey")).Text;
            string rank = ((Label)_gridView.Rows[nCurrentRow].FindControl("lblRank")).Text;
            string crewtype = ((Label)_gridView.Rows[nCurrentRow].FindControl("lblCrewType")).Text;
            string nationality = ((Label)_gridView.Rows[nCurrentRow].FindControl("lblNationality")).Text;
            string certcomp = ((Label)_gridView.Rows[nCurrentRow].FindControl("lblCertComp")).Text;
            string issuecountry = ((Label)_gridView.Rows[nCurrentRow].FindControl("lblIssuingCountry")).Text;
            string admintaccpt = ((Label)_gridView.Rows[nCurrentRow].FindControl("lblAdminAccpt")).Text;
            string tankercert = ((Label)_gridView.Rows[nCurrentRow].FindControl("lblTankerCert")).Text;
            string stcwpara = ((Label)_gridView.Rows[nCurrentRow].FindControl("lblStcwPara")).Text;
            string radioqual = ((Label)_gridView.Rows[nCurrentRow].FindControl("lblRadioQual")).Text;
            string yrsoperator = ((Label)_gridView.Rows[nCurrentRow].FindControl("lblYrsOperator")).Text;
            string yrsrank = ((Label)_gridView.Rows[nCurrentRow].FindControl("lblYrsRank")).Text;
            string tankertypeexp = ((Label)_gridView.Rows[nCurrentRow].FindControl("lblYrsTankerType")).Text;
            string alltankertypeexp = ((Label)_gridView.Rows[nCurrentRow].FindControl("lblYrsAllType")).Text;
            string joindate = ((Label)_gridView.Rows[nCurrentRow].FindControl("lblJoinDate")).Text;
            string engprof = ((Label)_gridView.Rows[nCurrentRow].FindControl("lblEngProf")).Text;
            string YrsWatch = ((Label)_gridView.Rows[nCurrentRow].FindControl("lblYrsWatch")).Text;

            if (ViewState["IDXDOC"] != null)
            {
                PhoenixCrewOCIMF.DeleteCrewDocument(new Guid(ViewState["IDXDOC"].ToString()), General.GetNullableGuid(keyid), crewtype, rank, nationality, certcomp, issuecountry, admintaccpt, tankercert, stcwpara, radioqual
            , decimal.Parse(yrsoperator), decimal.Parse(yrsrank), decimal.Parse(tankertypeexp), decimal.Parse(alltankertypeexp), DateTime.Parse(joindate), engprof, decimal.Parse(YrsWatch));

                ucStatus.Text = "Crew document scheduled to delete";
            }

            _gridView.EditIndex = -1;
            BindSire();
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }

    private void ShowNoRecordsFound(DataTable dt, GridView gv)
    {
        dt.Rows.Add(dt.NewRow());
        gv.DataSource = dt;
        gv.DataBind();

        int colcount = gv.Columns.Count;
        gv.Rows[0].Cells.Clear();
        gv.Rows[0].Cells.Add(new TableCell());
        gv.Rows[0].Cells[0].ColumnSpan = colcount;
        gv.Rows[0].Cells[0].HorizontalAlign = HorizontalAlign.Center;
        gv.Rows[0].Cells[0].ForeColor = System.Drawing.Color.Red;
        gv.Rows[0].Cells[0].Font.Bold = true;
        gv.Rows[0].Cells[0].Text = "NO RECORDS FOUND";
        gv.Rows[0].Attributes["onclick"] = "";
    }


}