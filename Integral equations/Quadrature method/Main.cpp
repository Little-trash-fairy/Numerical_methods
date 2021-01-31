#include <wx/wx.h>
#include <wx/dataview.h>

#include <vector>
using Vector = std::vector<double>;
using Matrix = std::vector<Vector>;

const int V = 5;
const int Lambda = 1;
const double a = 0;
const double b = 1;

double A(double x, double t)
{
    return x * t + x * x * t * t + x * x * x * t * t * t;
}

double f(double x)
{
    return V * (4.0 * x / 3 + x * x / 4 + x * x * x / 5);
}

double exact(double x)
{
    return V * x;
}


std::vector<double> m_Gauss(Matrix matrix)
{
    size_t n = matrix.size();
    Vector X(n);
    double z;
    //прямой ход метода Гаусса
    for (size_t i = 0; i < n; i++)
    {
        for (size_t j = i; j < n; j++)
        {
            z = (j != i) ? -matrix[j][i] / matrix[i][i] : 
                                            matrix[i][i];
            if (z != 0)
            {
                for (size_t k = i; k < n + 1; k++)
                {
                    matrix[j][k] = (j == i) ? matrix[j][k] / z : 
                                matrix[j][k] + matrix[i][k] * z;
                }
            }
        }
    }

    //обратный ход
    for (int i = n - 1; i >= 0; i--)
    {
        X[i] = matrix[i][n];
        for (int j = n - 1; j != i; j--)
        {
            X[i] = X[i] - matrix[i][j] * X[j];
        }
    }
    return X;
}

class MyFrame : public wxFrame
{
public:
    MyFrame(const wxString& title) : wxFrame(NULL, wxID_ANY, title)
    {
        Maximize();

        wxBoxSizer* topSizer;
        topSizer = new wxBoxSizer(wxHORIZONTAL);

        wxBoxSizer* widgetSizer;
        widgetSizer = new wxBoxSizer(wxVERTICAL);

        wxGridSizer* gridSizer;
        gridSizer = new wxGridSizer(0, 2, 0, 0);

        m_staticText1 = new wxStaticText(this, wxID_ANY, wxT("n"), wxDefaultPosition, wxDefaultSize, 0);
        m_staticText1->Wrap(-1);
        gridSizer->Add(m_staticText1, 0, wxALL, 5);

        m_IN = new wxTextCtrl(this, wxID_ANY, wxEmptyString, wxDefaultPosition, wxDefaultSize, 0);
        gridSizer->Add(m_IN, 0, wxALL, 5);


        widgetSizer->Add(gridSizer, 0, wxEXPAND, 5);

        m_BEval = new wxButton(this, wxID_ANY, wxT("Расчет"), wxDefaultPosition, wxDefaultSize, 0);
        widgetSizer->Add(m_BEval, 0, wxALL | wxALIGN_CENTER_HORIZONTAL, 5);


        topSizer->Add(widgetSizer, 0, wxEXPAND, 5);

        m_dataView = new wxDataViewListCtrl(this, wxID_ANY, wxDefaultPosition, wxDefaultSize, 0);
        m_dataViewListColumn1 = m_dataView->AppendTextColumn(wxT("Точка"), wxDATAVIEW_CELL_INERT, -1, static_cast<wxAlignment>(wxALIGN_CENTER), 0);
        m_dataViewListColumn2 = m_dataView->AppendTextColumn(wxT("Численное решение"), wxDATAVIEW_CELL_INERT, -1, static_cast<wxAlignment>(wxALIGN_CENTER), 0);
        m_dataViewListColumn3 = m_dataView->AppendTextColumn(wxT("Точное решение"), wxDATAVIEW_CELL_INERT, -1, static_cast<wxAlignment>(wxALIGN_CENTER), 0);
        m_dataViewListColumn4 = m_dataView->AppendTextColumn(wxT("Погрешность"), wxDATAVIEW_CELL_INERT, -1, static_cast<wxAlignment>(wxALIGN_CENTER), 0);
        topSizer->Add(m_dataView, 1, wxALL | wxEXPAND, 5);


        this->SetSizer(topSizer);
        this->Layout();

        this->Centre(wxBOTH);

        auto width = m_dataView->GetSize().GetWidth() / 4;
        m_dataViewListColumn1->SetWidth(width);
        m_dataViewListColumn2->SetWidth(width);
        m_dataViewListColumn3->SetWidth(width);
        m_dataViewListColumn4->SetWidth(width);

        m_BEval->Bind(wxEVT_COMMAND_BUTTON_CLICKED,
            [this](wxCommandEvent& event)
            {
                //извлечение значений
                long n;
                if (!m_IN->GetValue().ToLong(&n) || n <= 1)
                {
                    wxMessageBox("Некорректное значение первого параметра");
                    return;
                }
                m_dataView->DeleteAllItems();

                auto vecX = Vector(n);
                auto h = (b - a) / n;
                for (long i = 0; i < n; i++)
                {
                    vecX[i] = i * h;
                }

                Matrix M = Matrix(n, Vector(n + 1));
                //заполняем матрицу
                for (int i = 0; i < n; i++)
                {
                    double x = vecX[i];
                    for (int j = 0; j < n; j++)
                    {
                        if (i != j)
                        {
                            M[i][j] = Lambda * h * A(x, vecX[j]);
                        }
                        else
                        {
                            M[i][j] = 1 + Lambda * h * A(x * h, vecX[j]);
                        }

                    }
                    M[i][n] = f(x);
                }
                auto vecY = m_Gauss(M);

                wxVector<wxVariant> data;
                for (long i = 0; i < n; i++)
                {
                    data.clear();
                    data.push_back(wxVariant(std::to_string(vecX[i])));
                    data.push_back(wxVariant(std::to_string(vecY[i])));
                    data.push_back(wxVariant(std::to_string(exact(vecX[i]))));
                    auto diff = std::abs(exact(vecX[i]) - vecY[i]);
                    data.push_back(wxVariant(std::to_string(diff)));
                    m_dataView->AppendItem(data);
                }
                this->Layout();
            });
    }

protected:
    wxStaticText* m_staticText1;
    wxTextCtrl* m_IN;
    wxButton* m_BEval;
    wxDataViewListCtrl* m_dataView;
    wxDataViewColumn* m_dataViewListColumn1;
    wxDataViewColumn* m_dataViewListColumn2;
    wxDataViewColumn* m_dataViewListColumn3;
    wxDataViewColumn* m_dataViewListColumn4;
};

class MyApp : public wxApp
{
public:
    virtual bool OnInit()
    {
        if (!wxApp::OnInit())
            return false;
        MyFrame* frame = new MyFrame("Квадратурный метод");
        frame->Show(true);
        return true;
    }
};

wxIMPLEMENT_APP(MyApp);