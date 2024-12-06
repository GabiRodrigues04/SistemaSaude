using Microsoft.AspNetCore.Mvc;
using SistemaSaude.Models;
using System.Diagnostics;
using System.Data;
using System.Data.SqlClient;

namespace SistemaSaude.Controllers
{
    public class HomeController : Controller
    {
        private readonly string connection = "Server=DESKTOP-2I0FSTB\\GABSQL;Database=SistemaSaude; trusted_connection=true; trustservercertificate=true";

        public IActionResult Index()
        {
            List<Paciente> pacientes = new List<Paciente>();
            using (SqlConnection conn = new SqlConnection(connection))
            {
                string query = "SELECT * FROM Pacientes";
                SqlCommand cmd = new SqlCommand(query, conn);
                conn.Open();

                SqlDataReader reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    pacientes.Add(new Paciente
                    {
                        IdPaciente = (int)reader["IdPaciente"],
                        NomePaciente = reader["NomePaciente"].ToString(),
                        DataNasc = (DateTime)reader["DataNasc"],
                        Telefone = reader["Telefone"].ToString()
                    });
                }
            }
                return View(pacientes);
        }

        [HttpPost]
        public ActionResult CreatePaciente(Paciente paciente)
        {
            using (SqlConnection conn = new SqlConnection(connection))
            {
                string query = "INSERT INTO Pacientes (NomePaciente, DataNasc, Telefone) VALUES (@NomePaciente, @DataNasc, @Telefone)";
                SqlCommand cmd = new SqlCommand(query, conn);
                cmd.Parameters.AddWithValue("@NomePaciente", paciente.NomePaciente);
                cmd.Parameters.AddWithValue("@DataNasc", paciente.DataNasc);
                cmd.Parameters.AddWithValue("@Telefone", paciente.Telefone);
                conn.Open();
                cmd.ExecuteNonQuery();
            }
            return RedirectToAction("Index");
        }

        [HttpPost]
        public ActionResult DeletePaciente(int id)
        {
            using (SqlConnection conn = new SqlConnection(connection))
            {
                string query = "DELETE FROM Pacientes WHERE IdPaciente = @IdPaciente";
                SqlCommand cmd = new SqlCommand(query, conn);
                cmd.Parameters.AddWithValue("@IdPaciente", id);
                conn.Open();
                cmd.ExecuteNonQuery();
            }

            return RedirectToAction("Index");
        }

        public ActionResult UpdatePaciente(int id)
        {
            Paciente paciente = null;

            using (SqlConnection conn = new SqlConnection(connection))
            {
                string query = "SELECT * FROM Pacientes WHERE IdPaciente = @IdPaciente";
                SqlCommand cmd = new SqlCommand(query, conn);
                cmd.Parameters.AddWithValue("@IdPaciente", id);
                conn.Open();
                SqlDataReader reader = cmd.ExecuteReader();
                if (reader.Read())
                {
                    paciente = new Paciente
                    {
                        IdPaciente = (int)reader["IdPaciente"],
                        NomePaciente = reader["NomePaciente"].ToString(),
                        DataNasc = (DateTime)reader["DataNasc"],
                        Telefone = reader["Telefone"].ToString()
                    };
                }
            }
            return View(paciente);
        }

        [HttpPost]
        public ActionResult SaveUpdatePaciente(Paciente paciente)
        {

            using (SqlConnection conn = new SqlConnection(connection))
            {
                string query = "UPDATE Pacientes SET NomePaciente = @NomePaciente, DataNasc = @DataNasc, Telefone = @Telefone WHERE IdPaciente = @IdPaciente";
                SqlCommand cmd = new SqlCommand(query, conn);
                cmd.Parameters.AddWithValue("@IdPaciente", paciente.IdPaciente);
                cmd.Parameters.AddWithValue("@NomePaciente", paciente.NomePaciente);
                cmd.Parameters.AddWithValue("@DataNasc", paciente.DataNasc);
                cmd.Parameters.AddWithValue("@Telefone", paciente.Telefone);
                conn.Open();
                cmd.ExecuteNonQuery();
            }

            return RedirectToAction("Index");
        }

    }
}
