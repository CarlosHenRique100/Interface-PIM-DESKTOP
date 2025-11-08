using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Windows.Forms;

namespace PainelChamados
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            // Configura as colunas do DataGridView
            SetupDataGridView();

            // Carrega os dados de exemplo
            LoadData();
        }

        private void SetupDataGridView()
        {
            dgvChamados.AutoGenerateColumns = false;
            dgvChamados.Columns.Clear();

            // Coluna "Ver Detalhes" - Vamos pintá-la manualmente
            dgvChamados.Columns.Add(new DataGridViewTextBoxColumn
            {
                Name = "colDetalhes",
                HeaderText = "Ver Detalhes",
                Width = 100,
                ReadOnly = true
            });

            // Colunas de dados
            dgvChamados.Columns.Add(new DataGridViewTextBoxColumn
            {
                Name = "colId",
                HeaderText = "Id",
                DataPropertyName = "Id",
                Width = 80
            });

            dgvChamados.Columns.Add(new DataGridViewTextBoxColumn
            {
                Name = "colNome",
                HeaderText = "Nome",
                DataPropertyName = "Nome",
                AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill
            });

            // Coluna "Prioridade" - Também será pintada manualmente
            dgvChamados.Columns.Add(new DataGridViewTextBoxColumn
            {
                Name = "colPrioridade",
                HeaderText = "Prioridade",
                DataPropertyName = "Prioridade",
                Width = 120
            });

            dgvChamados.Columns.Add(new DataGridViewTextBoxColumn
            {
                Name = "colDataAbertura",
                HeaderText = "DataAbertura",
                DataPropertyName = "DataAbertura",
                DefaultCellStyle = new DataGridViewCellStyle { Format = "dd/MM/yyyy" },
                Width = 120
            });

            dgvChamados.Columns.Add(new DataGridViewTextBoxColumn
            {
                Name = "colStatus",
                HeaderText = "Status",
                DataPropertyName = "Status",
                Width = 150
            });
        }

        private void LoadData()
        {
            // Dados de exemplo baseados na sua imagem
            var listaChamados = new List<Chamado>
            {
                new Chamado { Id = 784512, Nome = "Ana Silva", Prioridade = "Alta", DataAbertura = new DateTime(2025, 11, 8), Status = "Aberto" },
                new Chamado { Id = 784511, Nome = "Bruno Costa", Prioridade = "Média", DataAbertura = new DateTime(2025, 11, 8), Status = "Aberto" },
                new Chamado { Id = 784510, Nome = "Carlos Souza", Prioridade = "Baixa", DataAbertura = new DateTime(2025, 11, 7), Status = "Em Andamento" },
                new Chamado { Id = 784508, Nome = "Eduardo Martins", Prioridade = "Alta", DataAbertura = new DateTime(2025, 11, 6), Status = "Aguardando..." },
                new Chamado { Id = 784507, Nome = "Fernanda Oliveira", Prioridade = "Baixa", DataAbertura = new DateTime(2025, 11, 6), Status = "Fechado" }
            };

            dgvChamados.DataSource = listaChamados;
        }

        // Este é o evento principal para personalizar o visual
        private void dgvChamados_CellPainting(object sender, DataGridViewCellPaintingEventArgs e)
        {
            // Ignora o cabeçalho
            if (e.RowIndex < 0) return;

            // --- Pinta a Coluna "Prioridade" ---
            if (e.ColumnIndex == dgvChamados.Columns["colPrioridade"].Index)
            {
                e.PaintBackground(e.CellBounds, true); // Pinta o fundo da célula

                string prioridade = e.Value as string;
                if (string.IsNullOrEmpty(prioridade))
                {
                    e.Handled = true;
                    return;
                }

                Color badgeColor;
                Color textColor = Color.White;

                // Define a cor baseada no valor
                switch (prioridade.ToLower())
                {
                    case "alta":
                        badgeColor = Color.FromArgb(220, 53, 69); // Vermelho
                        break;
                    case "média":
                        badgeColor = Color.FromArgb(255, 193, 7); // Laranja/Amarelo
                        break;
                    case "baixa":
                        badgeColor = Color.FromArgb(40, 167, 69); // Verde
                        break;
                    default:
                        badgeColor = Color.Gray;
                        break;
                }

                // Define a área do emblema (um pouco menor que a célula)
                int padding = 8;
                Rectangle badgeRect = new Rectangle(
                    e.CellBounds.X + padding,
                    e.CellBounds.Y + padding,
                    e.CellBounds.Width - (padding * 2),
                    e.CellBounds.Height - (padding * 2));

                int cornerRadius = 5; // Para cantos arredondados

                // Desenha o retângulo arredondado
                using (Brush badgeBrush = new SolidBrush(badgeColor))
                {
                    e.Graphics.SmoothingMode = SmoothingMode.AntiAlias;
                    RoundedRectangle.FillRoundedRectangle(e.Graphics, badgeRect, cornerRadius, badgeBrush);
                }

                // Desenha o texto centrado no emblema
                TextRenderer.DrawText(
                    e.Graphics,
                    prioridade,
                    e.CellStyle.Font,
                    badgeRect,
                    textColor,
                    TextFormatFlags.HorizontalCenter | TextFormatFlags.VerticalCenter
                );

                e.Handled = true; // Indica que já pintamos esta célula
            }
            // --- Pinta a Coluna "Ver Detalhes" ---
            else if (e.ColumnIndex == dgvChamados.Columns["colDetalhes"].Index)
            {
                e.PaintBackground(e.CellBounds, true); // Pinta o fundo

                // Desenha o bloco azul
                Color blockColor = Color.FromArgb(26, 35, 126); // Azul escuro
                int blockWidth = 60;
                int blockHeight = 30;

                // Centraliza o bloco na célula
                int blockX = e.CellBounds.X + (e.CellBounds.Width - blockWidth) / 2;
                int blockY = e.CellBounds.Y + (e.CellBounds.Height - blockHeight) / 2;
                Rectangle blockRect = new Rectangle(blockX, blockY, blockWidth, blockHeight);

                using (Brush b = new SolidBrush(blockColor))
                {
                    e.Graphics.FillRectangle(b, blockRect);
                }

                e.Handled = true; // Indica que já pintamos
            }
        }

        // Evento para simular o clique em "Ver Detalhes"
        private void dgvChamados_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0 && e.ColumnIndex == dgvChamados.Columns["colDetalhes"].Index)
            {
                // Pega o ID da linha clicada
                int id = (int)dgvChamados.Rows[e.RowIndex].Cells["colId"].Value;
                MessageBox.Show($"Você clicou em 'Ver Detalhes' para o chamado ID: {id}",
                                "Detalhes do Chamado",
                                MessageBoxButtons.OK,
                                MessageBoxIcon.Information);
            }
        }

        private void lblUser_Click(object sender, EventArgs e)
        {

        }
    }

    // Classe de modelo para os dados
    public class Chamado
    {
        public int Id { get; set; }
        public string Nome { get; set; }
        public string Prioridade { get; set; }
        public DateTime DataAbertura { get; set; }
        public string Status { get; set; }
    }
}