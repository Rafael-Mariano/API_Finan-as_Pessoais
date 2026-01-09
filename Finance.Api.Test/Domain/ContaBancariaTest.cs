using Xunit;
using Finance.Api.Domain;
using System.Drawing;

namespace Finance.Api.Test.Domain
{
    public class ContaBancariaTest
    {
        [Fact]
        public void NovaConta_RetornaValorZerado()
        {
            var conta = new ContaBancaria("Conta Teste");

            Assert.Equal(0m, conta.Saldo);
        }

        [Fact]
        public void Depositar_AdicionarValor_RetornaSaldoAtualizado()
        {
            var conta = new ContaBancaria("Conta Teste");

            conta.Depositar(100);

            Assert.Equal(100, conta.Saldo);
        }

        [Fact]
        public void Sacar_SubtrairValor_RetornaSaldoAtualizado()
        {
            var conta = new ContaBancaria("Conta Teste");

            conta.Depositar(50);
            conta.Sacar(20);

            Assert.Equal(30, conta.Saldo);
        }

        [Fact]
        public void Sacar_ValorMaiorQueSaldo_LancarExcessao()
        {
            var conta = new ContaBancaria("Conta Teste");

            conta.Depositar(100);
            conta.Sacar(100);

            Assert.Throws<InvalidOperationException>(() => conta.Sacar(100m));
        }

        [Fact]
        public void Transferir_MoverSaldoEntreContas()
        {
            // Arrange
            var origem = new ContaBancaria("Conta Origem");
            var destino = new ContaBancaria("Conta Destino");

            origem.Depositar(500m);

            // Act
            origem.Transferir(destino, 150m);

            // Assert
            Assert.Equal(350m, origem.Saldo);
            Assert.Equal(150m, destino.Saldo);
        }

        public void Transferir_ValorMaiorQueSaldo_LancarExcessao()
        {
            // Arrange
            var origem = new ContaBancaria("Conta Origem");
            var destino = new ContaBancaria("Conta Destino");

            origem.Depositar(250m);

            // Act and Assert
            Assert.Throws<InvalidOperationException>(() => origem.Transferir(destino, 300m));
        }

        [Fact]
        public void Depositar_RegistrarHistorico()
        {
            // Arrange
            var conta = new ContaBancaria("Conta Deposito");

            // Act
            conta.Depositar(240m);

            // Assert
            Assert.Single(conta.Historico);
            Assert.Equal("Depósito de: " + 240m, conta.Historico[0].Tipo);
            Assert.Equal(240m, conta.Historico[0].Valor);
        }

        [Fact]
        public void Sacar_RegistrarHistorico()
        {
            // Arrange
            var conta = new ContaBancaria("Conta Saque");

            conta.Depositar(100m);

            // Act
            conta.Sacar(30m);

            // Assert
            Assert.Equal(2, conta.Historico.Count);
            Assert.Equal("Saque de R$" + 30m, conta.Historico[1].Tipo);
            Assert.Equal(30m, conta.Historico[1].Valor);
        }

        [Fact]
        public void Transferir_RegistrarHistorico()
        {
            // Arrange
            var origem = new ContaBancaria("Conta Origem");
            var destino = new ContaBancaria("Conta Destino");

            origem.Depositar(250m);

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
            var conta = new ContaBancaria("Conta Extrato");

            conta.Depositar(350m);
            conta.Sacar(100m);

            // Act
            var saldoGeral = conta.Saldo;

            // Assert
            Assert.Equal(250m, saldoGeral);
        }
    }
}