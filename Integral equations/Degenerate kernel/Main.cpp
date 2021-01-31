#include <wx/wx.h>
#include <wx/dataview.h>

#include <vector>
#include <functional>
using Vector = std::vector<double>;
using Matrix = std::vector<Vector>;

const double V = 5;
const int Lambda = 1;

double f(double x)
{
    return V * (4.0 * x / 3 + x * x / 4 + x * x * x / 5);
}

//a{i}(x)
double funcA(int i, double x)
{
    return std::pow(x, i + 1);
}

//точное решение
double exact(double x)
{
    return V * x;
}

//альфы
double getAlpha(int i, int k)
{
    Matrix temp = {
        {1.0 / 3, 1.0 / 4, 1.0 / 5},
        {1.0 / 4, 1.0 / 5, 1.0 / 6},
        {1.0 / 5, 1.0 / 6, 1.0 / 7},
    };
    return temp[i][k];
}

//гаммы
double getG(int i)
{
    Vector temp = {
        V * (4.0 / 9 + 1.0 / 16 + 1.0 / 25),
        V * (4.0 / 12 + 1.0 / 20 + 1.0 / 30),
        V * (4.0 / 15 + 1.0 / 24 + 1.0 / 35)
    };
    return temp[i];
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
            if (j != i)
            {
                z = -matrix[j][i] / matrix[i][i];
            }
            else
            {
                z = matrix[i][i];
            }
            if (z != 0)
            {
                for (size_t k = i; k < n + 1; k++)
                {
                    if (j == i)
                    {
                        matrix[j][k] = matrix[j][k] / z;
                    }
                    else
                    {
                        matrix[j][k] = matrix[j][k] + matrix[i][k] * z;
                    }
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

        m_staticText1 = new wxStaticText(this, wxID_ANY, wxT("m"), wxDefaultPosition, wxDefaultSize, 0);
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
                long m;
                if (!m_IN->GetValue().ToLong(&m) || m <= 1)
                {
                    wxMessageBox("Некорректное значение первого параметра");
                    return;
                }
                m_dataView->DeleteAllItems();

                auto vecX = Vector(m);
                auto h = 1.0 / (m - 1);
                for (size_t i = 0; i < vecX.size(); i++)
                {
                    vecX[i] = i * h;
                }

                const int n = 3;
                Matrix M = Matrix(3, Vector(n + 1));
                //заполняем матрицу
                for (int i = 0; i < n; i++)
                {
                    for (int j = 0; j < n; j++)
                    {
                        if (i != j)
                        {
                            M[i][j] = Lambda * getAlpha(i, j);
                        }
                        else
                        {
                            M[i][j] = 1 + Lambda * getAlpha(i, j);
                        }
                    }
                    M[i][n] = getG(i);
                }
                auto vecQ = m_Gauss(M);

                auto vecY = Vector(vecX.size());
                for (size_t i = 0; i < vecX.size(); i++)
                {
                    vecY[i] = f(vecX[i]);

                    for (int j = 0; j < n; j++)
                    {
                        vecY[i] -= Lambda * vecQ[j] * funcA(j, vecX[i]);
                    }
                }

                wxVector<wxVariant> data;
                for (size_t i = 0; i < vecX.size(); i++)
                {
                    data.clear();
                    data.push_back(wxVariant(std::to_string(vecX[i])));
                    data.push_back(wxVariant(std::to_string(vecY[i])));
                    data.push_back(wxVariant(std::to_string(exact(vecX[i]))));
                    data.push_back(wxVariant(std::to_string(std::abs(exact(vecX[i]) - vecY[i]))));
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
        {
            return false;
        }
        MyFrame* frame = new MyFrame("Вырожденное ядро");
        frame->Show(true);
        return true;
    }
};

wxIMPLEMENT_APP(MyApp);