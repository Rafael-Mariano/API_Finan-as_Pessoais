using Xunit;
using Finance.Api.Domain;
using System.Drawing;

namespace Finance.Api.Test.Domain
{
    // Funcionalidades esperadas
    // * Criar contas com saldo zero OK
    // * Entrada de Valores OK
    // * Saída de Valores 
    // * Transferência entre contas
    // * Histórico de transações
    // * Extrato da conta
    public class ContaMovimentoTest
    {
        [Fact]
        public void NovaConta_RetornaValorZerado()
        {
            var conta = new ContaMovimento("Conta Teste");

            Assert.Equal(0m, conta.Saldo);
        }

        [Fact]
        public void Entrada_AdicionarValor_RetornaSaldo()
        {
            var conta = new ContaMovimento("Conta Teste");

            conta.Entrada(100);

            Assert.Equal(100, conta.Saldo);
        }

        [Fact]
        public void Saída_DiminuirValor_RetornaSaldo()
        {
            var conta = new ContaMovimento("Conta Teste");

            conta.Entrada(50);
            conta.Saida(20);

            Assert.Equal(30, conta.Saldo);
        }

        [Fact]
        public void Saida_ValorExcedente_Excessao()
        {
            // Arrange
            var conta = new ContaMovimento("Conta Teste");

            conta.Entrada(100);

            // Act and Assert
            Assert.Throws<InvalidOperationException>(() => conta.Saida(120m));
        }

        [Fact]
        public void Transferir_MoverSaldoEntreContas_RetornaSaldo()
        {
            // Arrange
            var origem = new ContaMovimento("Conta Origem");
            var destino = new ContaMovimento("Conta Destino");

            origem.Entrada(500m);

            // Act
            origem.Transferir(destino, 150m);

            // Assert
            Assert.Equal(350m, origem.Saldo);
            Assert.Equal(150m, destino.Saldo);
        }

        [Fact]
        public void Transferir_ValorExcedente_Excessao()
        {
            // Arrange
            var origem = new ContaMovimento("Conta Origem");
            var destino = new ContaMovimento("Conta Destino");

            origem.Entrada(250m);

            // Act and Assert
            Assert.Throws<InvalidOperationException>(() => origem.Transferir(destino, 300m));
        }

        [Fact]
        public void Entrada_RegistrarHistorico()
        {
            // Arrange
            var conta = new ContaMovimento("Conta Deposito");

            // Act
            conta.Entrada(240m);

            // Assert
            Assert.Single(conta.Historico);
            Assert.Equal("Entrada de: " + 240m, conta.Historico[0].Tipo);
            Assert.Equal(240m, conta.Historico[0].Valor);
        }

        [Fact]
        public void Saida_RegistrarHistorico()
        {
            // Arrange
            var conta = new ContaMovimento("Conta Saque");

            conta.Entrada(100m);

            // Act
            conta.Saida(30m);

            // Assert
            Assert.Equal(2, conta.Historico.Count);
            Assert.Equal("Saída de R$" + 30m, conta.Historico[1].Tipo);
            Assert.Equal(30m, conta.Historico[1].Valor);
        }

        [Fact]
        public void Transferir_RegistrarHistorico()
        {
            // Arrange
            var origem = new ContaMovimento("Conta Origem");
            var destino = new ContaMovimento("Conta Destino");

            origem.Entrada(250m);

            // Act
            origem.Transferir(destino, 200m);

            // Assert - Origem
            Assert.Equal($"Transferência de R$200 enviada para Conta Destino.", origem.Historico[1].Tipo);
            Assert.Equal(200m, origem.Historico[1].Valor);

            // Assert - Destino
            Assert.Equal($"Transferência de R$200 recebida de Conta Origem", destino.Historico[0].Tipo);
            Assert.Equal(200m, destino.Historico[0].Valor);
        }

        [Fact]
        public void ExtratoConta_SaldoGeral()
        {
            // Arrange
            var conta = new ContaMovimento("Conta Extrato");

            conta.Entrada(350m);
            conta.Saida(100m);

            // Act
            var saldoGeral = conta.Saldo;

            // Assert
            Assert.Equal(250m, saldoGeral);
        }
    }
}